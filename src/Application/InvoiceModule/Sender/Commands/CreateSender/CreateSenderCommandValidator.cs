using FluentValidation;

namespace Application.Invoice.Sender.Commands;

public class CreateSenderCommandValidator : AbstractValidator<CreateSenderCommand>
{
    public CreateSenderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("User ID is required.")
            .Must(NotBeWhiteSpace).WithMessage("User ID cannot be whitespace.");

        RuleFor(x => x.BusinessName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Business name is required.")
            .Must(NotBeWhiteSpace).WithMessage("Business name cannot be whitespace.")
            .MaximumLength(100).WithMessage("Business name cannot exceed 100 characters.");

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required.")
            .Must(NotBeWhiteSpace).WithMessage("Email cannot be whitespace.")
            .MaximumLength(320).WithMessage("Email cannot exceed 320 characters.")
            .EmailAddress().WithMessage("Email format is invalid.");

        RuleFor(x => x.AddressLine1)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Address line 1 is required.")
            .Must(NotBeWhiteSpace).WithMessage("Address line 1 cannot be whitespace.")
            .MaximumLength(200).WithMessage("Address line 1 cannot exceed 200 characters.");

        RuleFor(x => x.City)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("City is required.")
            .Must(NotBeWhiteSpace).WithMessage("City cannot be whitespace.")
            .MaximumLength(100).WithMessage("City cannot exceed 100 characters.");

        RuleFor(x => x.State)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("State is required.")
            .Must(NotBeWhiteSpace).WithMessage("State cannot be whitespace.")
            .MaximumLength(100).WithMessage("State cannot exceed 100 characters.");

        RuleFor(x => x.PostalCode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Postal code is required.")
            .Must(NotBeWhiteSpace).WithMessage("Postal code cannot be whitespace.")
            .MaximumLength(20).WithMessage("Postal code cannot exceed 20 characters.");

        RuleFor(x => x.Country)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Country is required.")
            .Must(NotBeWhiteSpace).WithMessage("Country cannot be whitespace.")
            .MaximumLength(100).WithMessage("Country cannot exceed 100 characters.");
    }

    private static bool NotBeWhiteSpace(string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }
}
