namespace Application.Invoice.Sender.Queries;

public record SenderResult(
    Guid Id,
    string BusinessName,
    string? LegalName,
    string Email,
    string? Phone,
    string? Website,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string PostalCode,
    string Country,
    string? TaxId,
    string? LogoUrl,
    string? PrimaryColor,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);