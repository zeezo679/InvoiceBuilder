namespace Api.Requests.Senders;

public record class UpdateBusinessInfoRequest
{
    public string BusinessName { get; init; } = null!;
    public string? LegalName { get; init; }
}
