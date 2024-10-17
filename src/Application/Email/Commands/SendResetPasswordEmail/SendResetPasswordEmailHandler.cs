using Application.Email.Responses;
using Application.Persistance.Interfaces.Account;
using Application.Persistance.Interfaces.Email;
using MediatR;

namespace Application.Email.Commands.SendResetPasswordEmail;

public sealed class SendResetPasswordEmailHandler : IRequestHandler<SendResetPasswordEmailCommand, SendResetPasswordEmailResponse>
{
    private readonly IAccountService _accountService;
    private readonly IEmailService _emailService;

    public SendResetPasswordEmailHandler(IAccountService accountService, IEmailService emailService)
    {
        _emailService = emailService;
        _accountService = accountService;
    }
    
    public async Task<SendResetPasswordEmailResponse> Handle(SendResetPasswordEmailCommand request, CancellationToken cancellationToken)
    {
        var resetPasswordData = await _accountService.GenerateResetPasswordTokenAsync(request.Email, cancellationToken);

        var url =
            $"<a href=http://localhost:3000/reset-password?token={resetPasswordData.Token}&userId={resetPasswordData.UserId}>Zresetuj hasło</a>";

        var message = "To reset password click link below: " + Environment.NewLine + url;

        await _emailService.SendEmailAsync(request.Email, "Reset password", message);

        return new SendResetPasswordEmailResponse($"Check {request.Email} to reset you password");
    }
}