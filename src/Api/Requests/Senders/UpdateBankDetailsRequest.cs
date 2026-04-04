namespace Api.Requests.Senders;

public record class UpdateBankDetailsRequest
{
    public string BankName { get; init; } = null!;
    public string BankAccountNumber { get; init; } = null!;
    public string BankRoutingNumber { get; init; } = null!;
    public string? Iban { get; init; }
    public string? Swift { get; init; }
}
