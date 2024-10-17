using Domain.AuthTokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities;

public class Account : IdentityUser<Guid>
{
    public string Name { get; set; }
    public string? SecondName { get; set; }
    public string Surname { get; set; }
    public string Pesel { get; set; }
    public DateOnly BirthDate { get; set; }

    public IReadOnlyCollection<UserRefreshToken> RefreshTokens => _refreshToken;

    private List<UserRefreshToken> _refreshToken = new();

    public void AddRefreshToken(RefreshToken refreshToken)
    {
        var token = UserRefreshToken.Create(refreshToken.Token, refreshToken.Expires);
        _refreshToken.Add(token);
    }

    public void DeleteRefreshToken(UserRefreshToken refreshToken)
    {
        _refreshToken.Remove(refreshToken);
    }
}

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        
    }
}