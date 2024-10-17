using Application.Persistance.Interfaces.Email;
using Infrastructure.Persistance.Email.Config;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace Infrastructure.Persistance.Email.Services;

public class EmailService : IEmailService
{
    private readonly SmtpConfig _smtpConfig;

    public EmailService(SmtpConfig smtpConfig)
    {
        _smtpConfig = smtpConfig;
    }
    
    public async Task SendEmailAsync(string email, string subject, string body)
    {
        var emailMessage = CreateEmailMessage(email, body, subject, _smtpConfig);
        
        using var client = new SmtpClient();
        {
            await client.ConnectAsync(_smtpConfig.SmtpHost, _smtpConfig.SmtpPort, true);
            await client.AuthenticateAsync(_smtpConfig.SmtpUser, _smtpConfig.SmtpPassword);
            var status = await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }

    private MimeMessage CreateEmailMessage(string email, string body, string subject, SmtpConfig smtpConfig)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(MailboxAddress.Parse(smtpConfig.SmtpFrom));
        emailMessage.To.Add(MailboxAddress.Parse(email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(TextFormat.Html) { Text = body };

        return emailMessage;
    }
}