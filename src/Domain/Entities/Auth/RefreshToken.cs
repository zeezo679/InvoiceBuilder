namespace Domain.Entities.Auth;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public DateTime Expires { get; set; }
    public bool IsRevoked { get; set; }
    public string UserId { get; set; } = null!;
    
}