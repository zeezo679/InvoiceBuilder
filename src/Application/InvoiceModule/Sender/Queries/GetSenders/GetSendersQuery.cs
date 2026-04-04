using Application.Common.DTOs;
using Application.Common.Models;
using Application.Invoice.Sender.Commands;
using ErrorOr;
using MediatR;

namespace Application.Invoice.Sender.Queries;

public sealed record GetSendersQuery(string UserId, bool? IsActive, int Page, int PageSize) : IRequest<ErrorOr<PagedResult<SenderDto>>>;

