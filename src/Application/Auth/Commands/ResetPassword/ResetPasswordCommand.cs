using MediatR;
using ErrorOr;

namespace Application.Auth.ResetPassword;

public sealed record ResetPasswordCommand(string Email, string Token, string NewPassword, string ConfirmPassword) : IRequest<ErrorOr<Unit>>;