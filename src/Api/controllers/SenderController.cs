using System.Security.Claims;
using Api.requests.Invoice;
using Application.Invoice.Sender.Commands;
using Application.Invoice.Sender.Queries;
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
            createSenderResult => CreatedAtAction(nameof(GetSenders), new { id = createSenderResult.Id }, createSenderResult),
            Problem);
        
    }   
    
    [HttpGet]
    public async Task<IActionResult> GetSenders([FromQuery] string userId, [FromQuery] bool? isActive)
    {
        var query = new GetSendersQuery(userId, isActive ?? false);
        var result = await _mediator.Send(query);

        return result.Match(
            senders => Ok(senders),
            Problem);   
    }
}


