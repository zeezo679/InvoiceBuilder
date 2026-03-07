using FluentValidation;

namespace Application.Auth.VerifiyEmail;

public class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
{
    public VerifyEmailCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull().WithMessage("UserId passed is null")
            .NotEmpty().WithMessage("UserId cannot be empty");
        
        RuleFor(x => x.Token)
            .NotNull().WithMessage("UserId passed is null")
            .NotEmpty().WithMessage("UserId cannot be empty");
    }
}