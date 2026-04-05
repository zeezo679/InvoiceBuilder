using System;
using FluentValidation;
using InvoiceBuilder.Application.Senders.Commands.UpdateBankDetails;

namespace Application.InvoiceModule.Sender.Commands.UpdateSender.UpdateBankDetails;

public class UpdateBankDetailsCommandValidator : AbstractValidator<UpdateBankDetailsCommand>
{
    public UpdateBankDetailsCommandValidator()
    {
        RuleFor(x => x.SenderId)
            .NotEmpty().WithMessage("SenderId is required.")
            .Must(id => id != Guid.Empty).WithMessage("SenderId cannot be an empty GUID.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.BankName)
            .NotEmpty().WithMessage("BankName is required.");

        RuleFor(x => x.BankAccountNumber)
            .NotEmpty().WithMessage("AccountNumber is required.");

        RuleFor(x => x.BankRoutingNumber)
            .NotEmpty().WithMessage("RoutingNumber is required.");
    }
}
