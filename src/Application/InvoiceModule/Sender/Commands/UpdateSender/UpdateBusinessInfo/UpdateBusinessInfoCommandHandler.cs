using Application.Common.Interfaces;
using Domain.Errors;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InvoiceBuilder.Application.Senders.Commands.UpdateBusinessInfo;

public sealed class UpdateBusinessInfoCommandHandler : IRequestHandler<UpdateBusinessInfoCommand, ErrorOr<Updated>>
{
    private readonly IAppDbContext _context;

    public UpdateBusinessInfoCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<Updated>> Handle(UpdateBusinessInfoCommand request, CancellationToken cancellationToken)
    {
        var sender = await _context.Senders.FirstOrDefaultAsync(
            s => s.Id == request.SenderId && s.UserId == request.UserId && !s.IsDeleted,
            cancellationToken);

        if (sender is null)
            return SenderErrors.NotFound;

        var result = sender.UpdateBusinessInfo(request.BusinessName, request.LegalName);
        if (result.IsError)
            return result.Errors;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Updated;
    }
}
