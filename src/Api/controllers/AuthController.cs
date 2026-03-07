using Api.requests;
using Application.Auth.Register;
using Application.Auth.VerifiyEmail;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
}


