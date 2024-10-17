using Application.Persistance.Interfaces.Account;
using Domain.AuthTokens;
using MediatR;

namespace Application.Account.Commands.RefreshToken;

public sealed class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, JsonWebToken>
{
    private readonly IAccountService _accountService;

    public RefreshTokenHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    public async Task<JsonWebToken> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await _accountService.RefreshToken(request.RefreshToken, cancellationToken);
    }
}