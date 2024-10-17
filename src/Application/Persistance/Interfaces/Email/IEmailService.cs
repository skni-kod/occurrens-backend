namespace Application.Persistance.Interfaces.Email;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string body);
}