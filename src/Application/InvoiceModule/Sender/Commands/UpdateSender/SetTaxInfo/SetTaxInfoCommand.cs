using ErrorOr;
using MediatR;

namespace InvoiceBuilder.Application.Senders.Commands.SetTaxInfo;

public sealed record SetTaxInfoCommand(
    Guid SenderId,
    string UserId,
    string TaxId,
    string? RegistrationNumber) : IRequest<ErrorOr<Updated>>;
