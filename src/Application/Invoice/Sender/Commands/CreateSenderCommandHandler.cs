namespace Application.Invoice.Sender.Commands;

using System;
using MediatR;
using ErrorOr;
using Domain.Entities;
using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Errors;

public class CreateSenderCommandHandler : IRequestHandler<CreateSenderCommand, ErrorOr<CreateSenderResult>>
{
    private readonly IAppDbContext _context;

    public CreateSenderCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<CreateSenderResult>> Handle(CreateSenderCommand request, CancellationToken cancellationToken)
    {

        var exists = await _context.Senders.AnyAsync(s => s.UserId == request.UserId && !s.IsDeleted, cancellationToken);

        if (exists)
            return SenderErrors.SenderAlreadyExists;

        var sender = Sender.Create
        (
            request.UserId,
            request.BusinessName,
            request.Email,
            request.AddressLine1,
            request.City,
            request.State,
            request.PostalCode,
            request.Country);


        if (sender.IsError)
            return sender.Errors;

        _context.Senders.Add(sender.Value);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateSenderResult { Id = sender.Value.Id };
    }
}

