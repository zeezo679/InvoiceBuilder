using Application.Invoice.Sender.Commands;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Sender.Queries;

public sealed record GetSendersQuery(string UserId, bool IsActive) : IRequest<ErrorOr<List<SenderResult>>>;

