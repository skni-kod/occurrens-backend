using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Application.Account.Enums;
using Application.Email.Dtos;
using Application.Persistance.Interfaces.Account;
using Application.Persistance.Interfaces.CurrentUser;
using Domain.Authentication;
using Domain.AuthTokens;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Roles;
using Infrastructure.Persistance.Accounts.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Infrastructure.Persistance.Accounts.Services;

public class AccountService : IAccountService
{
    private readonly OccurrensDbContext _context;
    private readonly UserManager<Account> _userManager;
    private readonly JwtSettings _jwtSettings;
    private IAccountService _accountServiceImplementation;
    private readonly SignInManager<Account> _signInManager;
    private readonly ICurrentUserService _currentUserService;

    public AccountService(OccurrensDbContext context, UserManager<Account> userManager, JwtSettings jwtSettings, SignInManager<Account> signInManager, ICurrentUserService currentUserService)
    {
        _context = context;
        _userManager = userManager;
        _jwtSettings = jwtSettings;
        _signInManager = signInManager;
        _currentUserService = currentUserService;
    }
    
    public async Task<Guid> CreateUserAsync(Account user, string password, EnumRole role, CancellationToken cancellationToken)
    {
        var isEmailExist = await _userManager.Users.AnyAsync(x => x.Email == user.Email, cancellationToken);
        if (isEmailExist) throw new BadRequestException("Wrong email!");

        var createUser = await _userManager.CreateAsync(user, password);
        if (!createUser.Succeeded) throw new CreateUserException(createUser.Errors);

        IdentityResult addUserRole;
        if (role == EnumRole.Doctor)
        {
            addUserRole = await _userManager.AddToRoleAsync(user, UserRoles.Doctor);
        }
        else
        {
            addUserRole = await _userManager.AddToRoleAsync(user, UserRoles.Patient);
        }

        if (!addUserRole.Succeeded) throw new BadRequestException("Add role filed!");

        var addClaim =
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        if (!addClaim.Succeeded) throw new BadRequestException("Add claim failed");
        
        

        await _context.SaveChangesAsync(cancellationToken);
        
        return user.Id;
    }

    public async Task<Account> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        if (user is null) throw new BadRequestException("User doesn't exist");

        return user;
    }

    public async Task<string> GenerateEmailConfirmTokenAsync(Account user, CancellationToken cancellationToken)
    {
        return await _userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public async Task ConfirmAccountAsync(Guid userId, string token, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (user is null) throw new BadRequestException("User doesn't exist!");

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (!result.Succeeded) throw new BadRequestException("Failed!");
    }

    public async Task<ResetPasswordDto> GenerateResetPasswordTokenAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        if (user is null) throw new BadRequestException("User not found");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        return new ResetPasswordDto
        {
            UserId = user.Id,
            Token = token
        };
    }

    public async Task ResetPasswordAsync(string token, Guid userId, string password, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        if (user is null) throw new BadRequestException("User doesn't exist!");

        var result = await _userManager.ResetPasswordAsync(user, token, password);
        if (!result.Succeeded) throw new CreateUserException(result.Errors);
    }

    public async Task<Account> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email)
            ?? throw new NotFoundException("Wrong data!");

        return user;
    }

    public async Task<JsonWebToken> SignIn(string email, string password, CancellationToken cancellationToken)
    {
        var user = await GetUserByEmailAsync(email, cancellationToken);

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, true);
        if (!result.Succeeded) throw new BadRequestException("Wrong data!");

        var userRoles = await _userManager.GetRolesAsync(user);
        var userClaims = await _userManager.GetClaimsAsync(user);

        var jwtToken = GenerateJsonWebToken(user, userRoles, userClaims);
        var refreshToken = GenerateRefreshToken();

        jwtToken.RefreshToken = refreshToken;
        DeleteExpiresRefreshToken(user);
        user.AddRefreshToken(refreshToken);

        _context.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        
        return jwtToken;
    }

    public async Task SignOut(string? refreshToken, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.Include(x => x.RefreshTokens)
                                                  .SingleOrDefaultAsync(x => x.Id == _currentUserService.UserId(), cancellationToken)
                                                ?? throw new NotFoundException("Nie znaleziono użytkownika");
        
        var token = user.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken);

        if (token is null)
        {
            throw new NotFoundException("Nie znaleziono tokenu");
        }
        
        user.DeleteRefreshToken(token);
        _context.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        await _signInManager.SignOutAsync();
    }

    public async Task<JsonWebToken> RefreshToken(string? refreshToken, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
                       .Include(x => x.RefreshTokens)
                       .SingleOrDefaultAsync(x => x.RefreshTokens.Any(y => y.Token == refreshToken), cancellationToken)
                   ?? throw new BadRequestException("Niepoprawny refresh token");
        
        var currentToken = user.RefreshTokens.Single(x => x.Token == refreshToken);

        if (currentToken.IsExpired)
        {
            throw new BadRequestException("Niepoprawny refresh token");
        }
        
        user.DeleteRefreshToken(currentToken);
        
        var userRoles = await _userManager.GetRolesAsync(user);
        var userClaims = await _userManager.GetClaimsAsync(user);

        var jwtToken = GenerateJsonWebToken(user, userRoles, userClaims);
        var newRefreshToken = GenerateRefreshToken();

        jwtToken.RefreshToken = newRefreshToken;
        DeleteExpiresRefreshToken(user);
        user.AddRefreshToken(newRefreshToken);

        _context.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        return jwtToken;
    }

    public JsonWebToken GenerateJsonWebToken(Account account, ICollection<string> roles, ICollection<Claim> claims)
    {
        var now = System.DateTime.UtcNow;

        var jwtClaims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, account.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString())
        };

        if (roles?.Any() is true)
        {
            foreach (var role in roles)
            {
                jwtClaims.Add(new Claim(ClaimTypes.Role, role));
            }
        }

        if (claims?.Any() is true)
        {
            var custromClaims = new List<Claim>();

            foreach (var claim in claims)
            {
                custromClaims.Add(new Claim(claim.Type, claim.Value));
            }
            jwtClaims.AddRange(custromClaims);
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = now.AddMinutes(_jwtSettings.ExpiryMinutes);

        var jwt = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            jwtClaims,
            expires: expires,
            signingCredentials: cred
            );

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new JsonWebToken
        {
            AccessToken = token,
            Expires = new DateTimeOffset(expires).ToUnixTimeSeconds(),
            UserId = account.Id,
            Email = account.Email,
            Roles = roles,
            BirthDate = account.BirthDate,
            Claims = claims?.ToDictionary(x => x.Type, c => c.Value),
            Name = account.Name,
            Pesel = account.Pesel,
            SecondName = account.SecondName,
            Surname = account.Surname
        };
    }

    public RefreshToken GenerateRefreshToken()
    {
        return new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpire)
        };
    }

    public void DeleteExpiresRefreshToken(Account user)
    {
        var expiredRefreshToken = user.RefreshTokens.Where(token => token.IsExpired).ToList();
        foreach (var token in expiredRefreshToken)
        {
            if (token.IsExpired)
            {
                user.DeleteRefreshToken(token);
            }
        }
    }
}
