using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Auth.Login;
using Application.Common.Interfaces;
using Application.Options;
using Domain.Entities.Auth;
using ErrorOr;
using Infrastructure.Data;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

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
    private readonly AppDbContext _context;
    private readonly JwtOptions _jwtOptions;

    public IdentityService(
        UserManager<ApplicationUser> userManager, 
        IConfiguration configuration,
        IEmailService emailService,
        IBackgroundJobService backgroundJobService,
        ITokenService tokenService,
        AppDbContext context,
        IOptions<JwtOptions> jwtOptions)
    {
        _userManager = userManager;
        _configuration = configuration;
        _emailService = emailService;
        _backgroundJobService = backgroundJobService;
        _tokenService = tokenService;
        _context = context;
        _jwtOptions = jwtOptions.Value;
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

        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);
        
        var claims =  BuildClaims(user);
        var token = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Persist refreshToken associated with user for later validation
        await SaveRefreshTokenAsync(user.Id, refreshToken);

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

    public async Task<ErrorOr<Success>> LogoutAsync(string userId, string refreshToken)
    {
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return Error.Failure("Identity.Logout", "User not found.");


        var storedRefreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.Token == refreshToken && !rt.IsRevoked);

        if (storedRefreshToken is null)
            return Error.Failure("Identity.Logout", "Invalid refresh token.");


        storedRefreshToken.IsRevoked = true;
        await _context.SaveChangesAsync();

        return Result.Success;
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

    private async Task SaveRefreshTokenAsync(string userId, string refreshToken)
    {
        _context.RefreshTokens.Add(new RefreshToken
        {
            UserId = userId,
            Token = refreshToken,
            IsRevoked = false,
            Expires = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays)
        });

        await _context.SaveChangesAsync();
    }
}