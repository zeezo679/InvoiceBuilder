using MediatR;
using ErrorOr;

namespace Application.Auth.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<ErrorOr<LoginResult>>;

