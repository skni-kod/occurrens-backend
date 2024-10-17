using Application.Persistance.Interfaces.Account;
using Application.Persistance.Interfaces.Email;
using MediatR;

namespace Application.Email.Commands.SendConfirmAccountEmail;

public sealed class SendConfirmAccountEmailHandler : IRequestHandler<SendConfirmAccountEmailCommand>
{
    private readonly IEmailService _emailService;
    private readonly IAccountService _accountService;

    public SendConfirmAccountEmailHandler(IEmailService emailService, IAccountService accountService)
    {
        _emailService = emailService;
        _accountService = accountService;
    }
    
    public async Task Handle(SendConfirmAccountEmailCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        var user = await _accountService.GetUserByIdAsync(request.UserId, cancellationToken);

        var token = await _accountService.GenerateEmailConfirmTokenAsync(user, cancellationToken);

        var link = $"<a href=http://localhost:3000/confirm-account?token={token}&userId={userId}>CLICK HERE</a>";
        
        var message = "Click link bellow to activate your account" + Environment.NewLine + link;

        await _emailService.SendEmailAsync(user.Email, "Active your account", message);
    }
}