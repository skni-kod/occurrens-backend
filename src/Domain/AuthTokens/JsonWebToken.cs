namespace Domain.AuthTokens;

public class JsonWebToken
{
    public string AccessToken { get; init; }
    public RefreshToken RefreshToken { get; set; }
    public long Expires { get; init; }
    public Guid UserId { get; init; }
    public string Email { get; set; }
    public DateOnly BirthDate { get; set; }
    public string Name { get; set; }
    public string? SecondName { get; set; } = string.Empty;
    public string Surname { get; set; }
    public string Pesel { get; set; }
    public ICollection<string> Roles { get; set; } = new List<string>();
    public IDictionary<string, string> Claims { get; init; } = new Dictionary<string, string>();
}