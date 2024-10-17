using Application.Persistance.Interfaces.Account;
using MediatR;

namespace Application.Account.Commands.SignOut;

public sealed class SignOutHandler : IRequestHandler<SignOutCommand>
{
    private readonly IAccountService _accountService;
    
    public SignOutHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    public async Task Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        await _accountService.SignOut(request.RefreshToken, cancellationToken);
    }
}