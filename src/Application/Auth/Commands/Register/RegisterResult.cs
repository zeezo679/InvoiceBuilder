namespace Application.Auth.Register;

public class RegisterResult(string userId, string email)
{
    public string UserId { get; } = userId;
    public string Email { get; } = email;
}