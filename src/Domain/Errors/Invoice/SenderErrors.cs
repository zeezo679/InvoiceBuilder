using ErrorOr;

namespace Domain.Errors;

public static class SenderErrors
{
    public static readonly Error Deleted =
        Error.Conflict("Sender.Deleted", "Operation cannot be performed on a deleted sender.");

    public static readonly Error EmptyUserId =
        Error.Validation("Sender.EmptyUserId", "User ID is required.");

    public static readonly Error EmptyBusinessName =
        Error.Validation("Sender.EmptyBusinessName", "Business name is required.");

    public static readonly Error InvalidEmail =
        Error.Validation("Sender.InvalidEmail", "Email is invalid.");

    public static readonly Error EmptyAddressLine1 =
        Error.Validation("Sender.EmptyAddressLine1", "Address line 1 is required.");

    public static readonly Error EmptyCity =
        Error.Validation("Sender.EmptyCity", "City is required.");

    public static readonly Error EmptyState =
        Error.Validation("Sender.EmptyState", "State is required.");

    public static readonly Error EmptyPostalCode =
        Error.Validation("Sender.EmptyPostalCode", "Postal code is required.");

    public static readonly Error EmptyCountry =
        Error.Validation("Sender.EmptyCountry", "Country is required.");

    public static readonly Error EmptyTaxId =
        Error.Validation("Sender.EmptyTaxId", "Tax ID is required.");

    public static readonly Error EmptyBankName =
        Error.Validation("Sender.EmptyBankName", "Bank name is required.");

    public static readonly Error EmptyBankAccountNumber =
        Error.Validation("Sender.EmptyBankAccountNumber", "Bank account number is required.");

    public static readonly Error EmptyBankRoutingNumber =
        Error.Validation("Sender.EmptyBankRoutingNumber", "Bank routing number is required.");

    public static readonly Error EmptyPrimaryColor =
        Error.Validation("Sender.EmptyPrimaryColor", "Primary color is required.");
}
