using FluentValidation;

namespace Application.Auth.VerifiyEmail;

public class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
{
    public VerifyEmailCommandValidator()
    {
        RuleFor(x => x.email)
            .NotNull().WithMessage("Email passed is null")
            .NotEmpty().WithMessage("Email cannot be empty");
        
        RuleFor(x => x.Token)
            .NotNull().WithMessage("Token passed is null")
            .NotEmpty().WithMessage("Token cannot be empty");
    }
}