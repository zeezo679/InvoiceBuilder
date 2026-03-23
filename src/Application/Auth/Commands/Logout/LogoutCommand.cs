using ErrorOr;
using MediatR;

namespace Application.Auth.Logout;

public sealed record LogoutCommand(string UserId, string RefreshToken) : IRequest<ErrorOr<Success>>;