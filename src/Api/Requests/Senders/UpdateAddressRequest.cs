namespace Api.Requests.Senders;

public record class UpdateAddressRequest
{
    public string AddressLine1 { get; init; } = null!;
    public string? AddressLine2 { get; init; }
    public string City { get; init; } = null!;
    public string State { get; init; } = null!;
    public string PostalCode { get; init; } = null!;
    public string Country { get; init; } = null!;
}
