namespace Api.requests.Invoice;

public record CreateSenderRequest(
    string BusinessName,
    string Email,
    string AddressLine1,
    string City,
    string State,
    string PostalCode,
    string Country);
