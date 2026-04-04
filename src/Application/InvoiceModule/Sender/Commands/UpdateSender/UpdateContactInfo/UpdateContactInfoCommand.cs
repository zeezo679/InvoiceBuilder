using ErrorOr;
using MediatR;

namespace InvoiceBuilder.Application.Senders.Commands.UpdateContactInfo;

public sealed record UpdateContactInfoCommand(
    Guid SenderId,
    string UserId,
    string Email,
    string? Phone,
    string? Website) : IRequest<ErrorOr<Updated>>;
