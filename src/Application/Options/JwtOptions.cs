namespace Application.Options;

public class JwtOptions
{
    public string Key { get; set; } = null!;
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationDays { get; set; }
}