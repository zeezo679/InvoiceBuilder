using System;
using FluentValidation;
using InvoiceBuilder.Application.Senders.Commands.UpdateBusinessInfo;

namespace Application.InvoiceModule.Sender.Commands.UpdateSender.UpdateBusinessInfo;

public class UpdateBusinessInfoCommandValidator : AbstractValidator<UpdateBusinessInfoCommand>
{
    public UpdateBusinessInfoCommandValidator()
    {
        RuleFor(x => x.SenderId)
            .NotEmpty().WithMessage("SenderId is required.")
            .Must(id => id != Guid.Empty).WithMessage("SenderId cannot be an empty GUID.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.BusinessName)
            .NotEmpty().WithMessage("BusinessName is required.");
    }
}
{

}
