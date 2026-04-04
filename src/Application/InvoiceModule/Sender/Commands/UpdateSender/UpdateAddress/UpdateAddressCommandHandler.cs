using Application.Common.Interfaces;
using Domain.Errors;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InvoiceBuilder.Application.Senders.Commands.UpdateAddress;

public sealed class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, ErrorOr<Updated>>
{
    private readonly IAppDbContext _context;

    public UpdateAddressCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<Updated>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var sender = await _context.Senders.FirstOrDefaultAsync(
            s => s.Id == request.SenderId && s.UserId == request.UserId && !s.IsDeleted,
            cancellationToken);

        if (sender is null)
            return SenderErrors.NotFound;

        var result = sender.UpdateAddress(
            request.AddressLine1,
            request.AddressLine2,
            request.City,
            request.State,
            request.PostalCode,
            request.Country);

        if (result.IsError)
            return result.Errors;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Updated;
    }
}
