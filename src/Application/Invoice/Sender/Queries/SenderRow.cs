namespace Application.Invoice.Sender.Queries;

// This is an internal class used to map the results from the database query. It is not exposed outside of this handler.
internal sealed class SenderRow
{
    public Guid Id { get; init; }
    public string BusinessName { get; init; } = string.Empty;
    public string? LegalName { get; init; }
    public string Email { get; init; } = string.Empty;
    public string? Phone { get; init; }
    public string City { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public string? LogoUrl { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public int TotalCount { get; init; }
}