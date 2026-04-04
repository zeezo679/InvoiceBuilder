using ErrorOr;
using MediatR;

namespace InvoiceBuilder.Application.Senders.Commands.UpdateBankDetails;

public sealed record UpdateBankDetailsCommand(
    Guid SenderId,
    string UserId,
    string BankName,
    string BankAccountNumber,
    string BankRoutingNumber,
    string? Iban,
    string? Swift) : IRequest<ErrorOr<Updated>>;
