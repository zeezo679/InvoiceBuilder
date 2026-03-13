using System.Data;
using FluentValidation;

namespace Application.Auth.Logout;

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull().WithMessage("User ID cannot be null.")
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.");
    }
}