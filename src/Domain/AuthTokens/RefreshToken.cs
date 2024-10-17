namespace Domain.AuthTokens;

public class RefreshToken
{
    public string Token { get; set; }
    public DateTimeOffset Expires { get; set; }
}