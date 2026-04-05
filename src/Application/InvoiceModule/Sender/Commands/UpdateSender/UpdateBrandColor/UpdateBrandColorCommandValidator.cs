using System;
using FluentValidation;
using InvoiceBuilder.Application.Senders.Commands.UpdateBrandColor;

namespace Application.InvoiceModule.Sender.Commands.UpdateSender.UpdateBrandColor;

public class UpdateBrandColorCommandValidator : AbstractValidator<UpdateBrandColorCommand>
{
    public UpdateBrandColorCommandValidator()
    {
        RuleFor(x => x.SenderId)
            .NotEmpty().WithMessage("SenderId is required.")
            .Must(id => id != Guid.Empty).WithMessage("SenderId cannot be an empty GUID.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.PrimaryColor)
            .NotEmpty().WithMessage("PrimaryColor is required.")
            .Matches("^#([0-9a-fA-F]{6}|[0-9a-fA-F]{3})$").WithMessage("PrimaryColor must be a valid hex color code.");
    }
}
