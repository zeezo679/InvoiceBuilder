using Application.Common.Interfaces;
using MediatR;
using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<LoginResult>>
{
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ErrorOr<LoginResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.LoginAsync(request.Email, request.Password);

            return result.Match<ErrorOr<LoginResult>>(
                loginResult => loginResult,
                error => error);
    }


}