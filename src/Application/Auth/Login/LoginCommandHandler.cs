using Application.Common.Interfaces;
using MediatR;
using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, ErrorOr<LoginResult>>
{
    private readonly ITokenService _tokenService;
    private readonly IAppDbContext _context;
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(ITokenService tokenService, IAppDbContext context, IIdentityService identityService)
    {
        _tokenService = tokenService;
        _context = context;
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