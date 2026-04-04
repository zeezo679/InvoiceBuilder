using Application.Common.DTOs;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Sender.Queries.GetSenderById;

public sealed record GetSenderByIdQuery(Guid SenderId, Guid UserId)
 : IRequest<ErrorOr<SenderDto>>;
