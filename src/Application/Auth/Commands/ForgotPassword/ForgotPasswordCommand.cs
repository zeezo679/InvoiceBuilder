using MediatR;
using ErrorOr;

namespace Application.Auth.ForgotPassword;

public sealed record ForgotPasswordCommand(string Email) : IRequest<ErrorOr<Unit>>;