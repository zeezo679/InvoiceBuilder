namespace Api.requests.Invoice;

public record class UpdateSenderBusinessInfoRequest
{
    public string BusinessName { get; init; } = null!;
    public string? LegalName { get; init; }
}
