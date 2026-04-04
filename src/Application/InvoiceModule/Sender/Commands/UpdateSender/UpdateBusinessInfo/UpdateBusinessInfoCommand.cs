using ErrorOr;
using MediatR;

namespace InvoiceBuilder.Application.Senders.Commands.UpdateBusinessInfo;

public sealed record UpdateBusinessInfoCommand(
    Guid SenderId,
    string UserId,
    string BusinessName,
    string? LegalName) : IRequest<ErrorOr<Updated>>;
