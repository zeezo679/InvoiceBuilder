using System;
using System.Data;
using FluentValidation;
using InvoiceBuilder.Application.Senders.Commands.SetTaxInfo;

namespace Application.InvoiceModule.Sender.Commands.UpdateSender.SetTaxInfo;


//  Guid SenderId,
    // string UserId,
    // string TaxId,
    // string? RegistrationNumber
public class SetTaxInfoCommandValidator : AbstractValidator<SetTaxInfoCommand>
{
    public SetTaxInfoCommandValidator()
    {
        RuleFor(x => x.SenderId)
            .NotEmpty().WithMessage("SenderId is required.")
            .Must(id => id != Guid.Empty).WithMessage("SenderId cannot be an empty GUID.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.TaxId)
            .NotEmpty().WithMessage("TaxId is required.");
    }
}

