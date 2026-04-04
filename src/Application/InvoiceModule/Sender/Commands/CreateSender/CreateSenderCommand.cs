using MediatR;
using ErrorOr;

namespace Application.Invoice.Sender.Commands;

public record class CreateSenderCommand(
	string UserId,
	string BusinessName,
	string Email,
	string AddressLine1,
	string City,
	string State,
	string PostalCode,
	string Country) : IRequest<ErrorOr<CreateSenderResult>>;

