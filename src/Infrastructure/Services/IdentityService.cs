using System.Text;
using Application.Common.Interfaces;
using ErrorOr;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IBackgroundJobService _backgroundJobService;

    public IdentityService(
        UserManager<ApplicationUser> userManager, 
        IConfiguration configuration,
        IEmailService emailService,
        IBackgroundJobService backgroundJobService
        )
    {
        _userManager = userManager;
        _configuration = configuration;
        _emailService = emailService;
        _backgroundJobService = backgroundJobService;
    }


    public async Task<ErrorOr<string>> CreateUserAsync(string firstName, string lastName, string email, string password)
    {
        var user = new ApplicationUser
        {
            UserName = $"{firstName}_{lastName}",
            FirstName = firstName,
            LastName = lastName,
            Email = email,
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            return Error.Failure("Identity.CreateUser", result.Errors.First().Description); 
        
        await SendVerificationEmailAsync(user);
        return user.Id;
    }

    private async Task SendVerificationEmailAsync(ApplicationUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        
        var baseUrl = _configuration["App:BaseUrl"];
        var parameters = new Dictionary<string, string>
        {
            { "token", encodedToken },
            { "UserId", user.Id }
        };
        
        var verificationUri = QueryHelpers
            .AddQueryString($"{baseUrl}/auth/verify-email", parameters);
        
        _backgroundJobService.Enqueue(() => _emailService.SendConfirmationEmailAsync(
            user.Email,
            "Email Verification", 
            verificationUri));
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);

        return existingUser is null ? false : true;
    }

    public async Task<bool> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return false;

        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }
}