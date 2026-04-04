using ErrorOr;
using MediatR;

namespace InvoiceBuilder.Application.Senders.Commands.UpdateAddress;

public sealed record UpdateAddressCommand(
    Guid SenderId,
    string UserId,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string PostalCode,
    string Country) : IRequest<ErrorOr<Updated>>;
