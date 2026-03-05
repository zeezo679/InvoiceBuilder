using Application.Common.Interfaces;
using ErrorOr;

namespace Infrastructure.Services;

using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }


    public async Task<ErrorOr<string>> CreateUserAsync(string firstName, string lastName, string email, string password)
    {
        var user = new ApplicationUser
        {
            UserName = $"{firstName} {lastName}",
            FirstName = firstName,
            LastName = lastName,
            Email = email,
        };

        var result = await _userManager.CreateAsync(user, password);

        return result.Succeeded
            ? user.Id
            : Error.Failure("Identity.CreateUser", result.Errors.First().Description);
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);

        return existingUser is null ? false : true;
    }
}