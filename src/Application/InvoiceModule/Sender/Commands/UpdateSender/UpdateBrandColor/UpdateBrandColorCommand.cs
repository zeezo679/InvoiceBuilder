using ErrorOr;
using MediatR;

namespace InvoiceBuilder.Application.Senders.Commands.UpdateBrandColor;

public sealed record UpdateBrandColorCommand(
    Guid SenderId,
    string UserId,
    string PrimaryColor) : IRequest<ErrorOr<Updated>>;
