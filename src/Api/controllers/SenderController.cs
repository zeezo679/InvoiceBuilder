using System.Security.Claims;
using Api.requests.Invoice;
using Application.Invoice.Sender.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.controllers;

[Route("senders")]
[ApiController]
[Authorize]
public class SenderController : ApiController
{
    private readonly IMediator _mediator;

    public SenderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateSender([FromBody] CreateSenderRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var command = new CreateSenderCommand(
            userId!,
            request.BusinessName,
            request.Email,
            request.AddressLine1,
            request.City,
            request.State,
            request.PostalCode,
            request.Country);
        
        var result = await _mediator.Send(command);

        return result.Match(
            createSenderResult => Ok(new { createSenderResult.Id }), //change after testing
            Problem);

    }   
    
}


