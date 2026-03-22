using System.Security.Claims;
using Api.requests;
using Application.Auth.ForgotPassword;
using Application.Auth.Login;
using Application.Auth.Logout;
using Application.Auth.Register;
using Application.Auth.VerifiyEmail;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Api.controllers;

[ApiController]
[Route("auth")]
public class AuthController : ApiController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var command = new RegisterUserCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);

        var result = await _mediator.Send(command);

        return result.Match(
            registerResult => Ok(new { registerResult.Email, registerResult.UserId }),
            Problem);
    }

    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] EmailVerificationRequest request)
    {
        var command = new VerifyEmailCommand(request.UserId, request.Token);

        var result = await _mediator.Send(command);
        
        return result.Match(
        _ => Ok(new { Message = "Email Verification Complete!"}),
        Problem);
    }

    [HttpPost("login")]
    [EnableRateLimiting("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {   
        var command = new LoginCommand(request.Email, request.Password);

        var result = await _mediator.Send(command);

        return result.Match(
            loginResult => Ok(new { loginResult.AccessToken, loginResult.RefreshToken }),
            Problem);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        //get userId from claims for better security, instead of passing it in the request body
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var command = new LogoutCommand(userId, request.RefreshToken);

        var result = await _mediator.Send(command);

        return result.Match(
            _ => NoContent(),
            Problem);
    }
    
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var command = new ForgotPasswordCommand(request.Email);

        var result = await _mediator.Send(command);

        return result.Match(
            _ => Ok(new { Message = "If an account with that email exists, a password reset link will be sent." }),
            Problem);
    }
}

