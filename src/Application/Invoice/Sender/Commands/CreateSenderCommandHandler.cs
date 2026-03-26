using System;
using MediatR;
using ErrorOr;
using Application.Common.Interfaces;

namespace Application.Invoice.Sender.Commands;

public class CreateSenderCommandHandler : IRequestHandler<CreateSenderCommand, ErrorOr<CreateSenderResult>>
{

    private readonly IAppDbContext _context;

    public CreateSenderCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public Task<ErrorOr<CreateSenderResult>> Handle(CreateSenderCommand request, CancellationToken cancellationToken)
    {
        
    }
}

