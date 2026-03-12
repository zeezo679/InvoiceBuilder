namespace Application.Auth.Login;

public class LoginResult
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}