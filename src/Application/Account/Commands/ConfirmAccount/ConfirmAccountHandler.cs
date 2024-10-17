using Application.Account.Responses;
using Application.Persistance.Interfaces.Account;
using MediatR;

namespace Application.Account.Commands.ConfirmAccount;

public sealed class ConfirmAccountHandler : IRequestHandler<ConfirmAccountCommand, ConfirmAccountResponse>
{
    private readonly IAccountService _accountService;
    
    public ConfirmAccountHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    public async Task<ConfirmAccountResponse> Handle(ConfirmAccountCommand request, CancellationToken cancellationToken)
    {
        await _accountService.ConfirmAccountAsync(request.UserId, request.Token, cancellationToken);

        return new ConfirmAccountResponse("Success!");
    }
}