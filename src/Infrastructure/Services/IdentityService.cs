using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Auth.Login;
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
    private readonly ITokenService _tokenService;

    public IdentityService(
        UserManager<ApplicationUser> userManager, 
        IConfiguration configuration,
        IEmailService emailService,
        IBackgroundJobService backgroundJobService,
        ITokenService tokenService
        )
    {
        _userManager = userManager;
        _configuration = configuration;
        _emailService = emailService;
        _backgroundJobService = backgroundJobService;
        _tokenService = tokenService;
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

    public async Task<ErrorOr<LoginResult>> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return Error.Failure("Identity.Login", "Invalid email or password.");

        if (!user.EmailConfirmed)
            return Error.Failure("Identity.Login", "Email not confirmed. Please verify your email before logging in.");
        
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);

        if (!isPasswordValid)
        {
            await _userManager.AccessFailedAsync(user);
            return Error.Failure("Identity.Login", "Invalid email or password.");
        }

        await _userManager.ResetAccessFailedCountAsync(user);

        var claims =  BuildClaims(user);
        var token = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // TODO: Persist refreshToken associated with user for later validation

        return new LoginResult
        {
            AccessToken = token,
            RefreshToken = refreshToken,
        };
        
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

    private IEnumerable<Claim> BuildClaims(ApplicationUser user)
    {
        var userClaims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        return userClaims;
    }
}