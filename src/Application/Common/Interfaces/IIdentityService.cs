using Application.Auth.Login;
using ErrorOr;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<ErrorOr<string>> CreateUserAsync(string firstName, string lastName, string email, string password);

    Task<ErrorOr<LoginResult>> LoginAsync(string email, string password);

    Task<bool> UserExistsAsync(string email);

    Task<bool> ConfirmEmailAsync(string email, string token);

    Task<ErrorOr<Success>> LogoutAsync(string userId, string refreshToken);

    Task<ErrorOr<Success>> ForgotPasswordAsync(string email);

    Task<ErrorOr<Success>> ResetPasswordAsync(string email, string token, string newPassword, string confirmPassword);
}