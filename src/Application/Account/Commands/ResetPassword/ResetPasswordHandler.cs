using Application.Account.Responses;
using Application.Persistance.Interfaces.Account;
using MediatR;

namespace Application.Account.Commands.ResetPassword;

public sealed class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordResponse>
{
    private readonly IAccountService _accountService;

    public ResetPasswordHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    public async Task<ResetPasswordResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        await _accountService.ResetPasswordAsync(request.Token, request.UserId, request.Password, cancellationToken);

        return new ResetPasswordResponse("Success!");
    }
}