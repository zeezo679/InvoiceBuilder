using System;
using FluentValidation;
using InvoiceBuilder.Application.Senders.Commands.UpdateContactInfo;

namespace Application.InvoiceModule.Sender.Commands.UpdateSender.UpdateContactInfo;

public class UpdateContactInfoCommandValidator : AbstractValidator<UpdateContactInfoCommand>
{
    public UpdateContactInfoCommandValidator()
    {
        RuleFor(x => x.SenderId)
            .NotEmpty().WithMessage("SenderId is required.")
            .Must(id => id != Guid.Empty).WithMessage("SenderId cannot be an empty GUID.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("PhoneNumber is required.");
    }
}
