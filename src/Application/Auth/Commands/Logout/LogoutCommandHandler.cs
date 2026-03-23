using Application.Common.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Auth.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ErrorOr<Success>>
{
    private readonly IIdentityService _identityService;

    public LogoutCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<ErrorOr<Success>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        return _identityService.LogoutAsync(request.UserId, request.RefreshToken);
    }
}