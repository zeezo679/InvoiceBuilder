using FluentValidation;

namespace Application.Invoice.Sender.Queries.GetSenderById;

public sealed class GetSenderByIdQueryValidator : AbstractValidator<GetSenderByIdQuery>
{
    public GetSenderByIdQueryValidator()
    {
        RuleFor(x => x.SenderId)
            .NotEqual(Guid.Empty).WithMessage("SenderId is required.");

        RuleFor(x => x.UserId)
            .NotEqual(Guid.Empty).WithMessage("UserId is required.");
    }
}
