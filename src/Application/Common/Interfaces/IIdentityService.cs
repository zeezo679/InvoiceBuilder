using ErrorOr;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<ErrorOr<string>> CreateUserAsync(string firstName, string lastName, string email, string password);

    Task<bool> UserExistsAsync(string email);

    Task<bool> ConfirmEmailAsync(string userId, string token);
}