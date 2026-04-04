namespace Api.Requests.Senders;

public record class SetTaxInfoRequest
{
    public string TaxId { get; init; } = null!;
    public string? RegistrationNumber { get; init; }
}
