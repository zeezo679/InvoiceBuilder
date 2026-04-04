using System;
using FluentValidation;

namespace Application.Invoice.Sender.Queries;

public class GetSendersQueryValidator : AbstractValidator<GetSendersQuery>
{
    public GetSendersQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .Must(BeAValidGuid).WithMessage("UserId must be a valid GUID.");
    }

    private bool BeAValidGuid(string userId)
    {
        return Guid.TryParse(userId, out _);
    }
}
