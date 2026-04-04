using Application.Common.Interfaces;
using Domain.Errors;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InvoiceBuilder.Application.Senders.Commands.UpdateContactInfo;

public sealed class UpdateContactInfoCommandHandler : IRequestHandler<UpdateContactInfoCommand, ErrorOr<Updated>>
{
    private readonly IAppDbContext _context;

    public UpdateContactInfoCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<Updated>> Handle(UpdateContactInfoCommand request, CancellationToken cancellationToken)
    {
        var sender = await _context.Senders.FirstOrDefaultAsync(
            s => s.Id == request.SenderId && s.UserId == request.UserId && !s.IsDeleted,
            cancellationToken);

        if (sender is null)
            return SenderErrors.NotFound;

        var result = sender.UpdateContactInfo(request.Email, request.Phone, request.Website);
        if (result.IsError)
            return result.Errors;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Updated;
    }
}
