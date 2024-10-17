namespace Application.Email.Dtos;

public class ResetPasswordDto
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
}