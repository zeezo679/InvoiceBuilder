using Application.Common.Interfaces;
using Domain.Errors;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InvoiceBuilder.Application.Senders.Commands.SetTaxInfo;

public sealed class SetTaxInfoCommandHandler : IRequestHandler<SetTaxInfoCommand, ErrorOr<Updated>>
{
    private readonly IAppDbContext _context;

    public SetTaxInfoCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<Updated>> Handle(SetTaxInfoCommand request, CancellationToken cancellationToken)
    {
        var sender = await _context.Senders.FirstOrDefaultAsync(
            s => s.Id == request.SenderId && s.UserId == request.UserId && !s.IsDeleted,
            cancellationToken);

        if (sender is null)
            return SenderErrors.NotFound;

        var result = sender.SetTaxInfo(request.TaxId, request.RegistrationNumber);
        if (result.IsError)
            return result.Errors;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Updated;
    }
}
