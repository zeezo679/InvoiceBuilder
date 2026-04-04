using System.Security.Claims;
using Api.Extensions;
using Api.Requests.Senders;
using Api.requests.Invoice;
using Application.Invoice.Sender.Commands;
using Application.Invoice.Sender.Queries;
using Application.Invoice.Sender.Queries.GetSenderById;
using InvoiceBuilder.Application.Senders.Commands.SetTaxInfo;
using InvoiceBuilder.Application.Senders.Commands.UpdateAddress;
using InvoiceBuilder.Application.Senders.Commands.UpdateBankDetails;
using InvoiceBuilder.Application.Senders.Commands.UpdateBrandColor;
using InvoiceBuilder.Application.Senders.Commands.UpdateBusinessInfo;
using InvoiceBuilder.Application.Senders.Commands.UpdateContactInfo;
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
    public async Task<IActionResult> GetSenders([FromQuery] string userId, [FromQuery] bool? isActive, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        pageSize = Math.Clamp(pageSize, 1, 50); //page size cannot be less than 1 or greater than 50
        page = Math.Max(page, 1); //page cannot be 0 or negative

        var query = new GetSendersQuery(userId, isActive ?? false, page, pageSize);
        var result = await _mediator.Send(query);

        return result.Match(
            senders => Ok(senders),
            Problem);   
    }

    [HttpGet("{senderId:guid}")]
    public async Task<IActionResult> GetSenderById([FromRoute] Guid senderId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var query = new GetSenderByIdQuery(senderId, userId);
        var result = await _mediator.Send(query);

        return result.Match(
            sender => Ok(sender),
            Problem);
    }

    [HttpPatch("{senderId:guid}/update-business-info")]
    public async Task<IActionResult> UpdateBusinessInfo([FromRoute] Guid senderId, [FromBody] UpdateBusinessInfoRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var command = new UpdateBusinessInfoCommand(senderId, userId, request.BusinessName, request.LegalName);
        var result = await _mediator.Send(command);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPatch("{senderId:guid}/update-contact-info")]
    public async Task<IActionResult> UpdateContactInfo([FromRoute] Guid senderId, [FromBody] UpdateContactInfoRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var command = new UpdateContactInfoCommand(senderId, userId, request.Email, request.Phone, request.Website);
        var result = await _mediator.Send(command);

        return result.Match(
            _ => NoContent(),
            Problem);

    }

    [HttpPatch("{senderId:guid}/update-address")]
    public async Task<IActionResult> UpdateAddress([FromRoute] Guid senderId, [FromBody] UpdateAddressRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var command = new UpdateAddressCommand(
            senderId,
            userId,
            request.AddressLine1,
            request.AddressLine2,
            request.City,
            request.State,
            request.PostalCode,
            request.Country);

        var result = await _mediator.Send(command);

        return result.Match(
            _ => NoContent(),
            Problem);

    }

    [HttpPatch("{senderId:guid}/set-tax-info")]
    public async Task<IActionResult> SetTaxInfo([FromRoute] Guid senderId, [FromBody] SetTaxInfoRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var command = new SetTaxInfoCommand(senderId, userId, request.TaxId, request.RegistrationNumber);
        var result = await _mediator.Send(command);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPatch("{senderId:guid}/update-bank-details")]
    public async Task<IActionResult> UpdateBankDetails([FromRoute] Guid senderId, [FromBody] UpdateBankDetailsRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var command = new UpdateBankDetailsCommand(
            senderId,
            userId,
            request.BankName,
            request.BankAccountNumber,
            request.BankRoutingNumber,
            request.Iban,
            request.Swift);

        var result = await _mediator.Send(command);

        return result.Match(
            _ => NoContent(),
            Problem);
    }

    [HttpPatch("{senderId:guid}/update-brand-color")]
    public async Task<IActionResult> UpdateBrandColor([FromRoute] Guid senderId, [FromBody] UpdateBrandColorRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var command = new UpdateBrandColorCommand(senderId, userId, request.PrimaryColor);
        var result = await _mediator.Send(command);

        return result.Match(
            _ => NoContent(),
            Problem);
    }
}
