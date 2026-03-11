using ErrorOr;

namespace Domain.Errors;


public static class CustomerErrors
{
    public static readonly Error Deleted =
        Error.Conflict("Customer.Deleted", "Operation cannot be performed on a deleted customer.");

    public static readonly Error EmptyUserId =
        Error.Validation("Customer.EmptyUserId", "User ID is required.");

    public static readonly Error EmptyCustomerName =
        Error.Validation("Customer.EmptyCustomerName", "Customer name is required.");

    public static readonly Error EmptyCompanyName =
        Error.Validation("Customer.EmptyCompanyName", "Company name is required.");

    public static readonly Error InvalidEmail =
        Error.Validation("Customer.InvalidEmail", "Email is invalid.");

    public static readonly Error EmptyEmail =
        Error.Validation("Customer.EmptyEmail", "Email is required.");

    public static readonly Error EmptyBillingAddressLine1 =
        Error.Validation("Customer.EmptyBillingAddressLine1", "Billing address line 1 is required.");

    public static readonly Error EmptyBillingCity =
        Error.Validation("Customer.EmptyBillingCity", "Billing city is required.");

    public static readonly Error EmptyBillingState =
        Error.Validation("Customer.EmptyBillingState", "Billing state is required.");

    public static readonly Error EmptyBillingPostalCode =
        Error.Validation("Customer.EmptyBillingPostalCode", "Billing postal code is required.");

    public static readonly Error EmptyBillingCountry =
        Error.Validation("Customer.EmptyBillingCountry", "Billing country is required.");

    public static readonly Error EmptyShippingCity =
        Error.Validation("Customer.EmptyShippingCity", "Shipping city is required when address is provided.");

    public static readonly Error EmptyShippingState =
        Error.Validation("Customer.EmptyShippingState", "Shipping state is required when address is provided.");

    public static readonly Error EmptyShippingPostalCode =
        Error.Validation("Customer.EmptyShippingPostalCode", "Shipping postal code is required when address is provided.");

    public static readonly Error EmptyShippingCountry =
        Error.Validation("Customer.EmptyShippingCountry", "Shipping country is required when address is provided.");

    public static readonly Error EmptyTaxExemptionNumber =
        Error.Validation("Customer.EmptyTaxExemptionNumber", "Tax exemption number is required for tax exempt customers.");

    public static readonly Error EmptyTaxExemptionCertificateUrl =
        Error.Validation("Customer.EmptyTaxExemptionCertificateUrl", "Tax exemption certificate URL is required for tax exempt customers.");

    public static readonly Error MissingTaxExemptionExpiryDate =
        Error.Validation("Customer.MissingTaxExemptionExpiryDate", "Tax exemption expiry date is required for tax exempt customers.");

    public static readonly Error ExpiredTaxExemption =
        Error.Validation("Customer.ExpiredTaxExemption", "Tax exemption expiry date must be in the future.");

    public static readonly Error NegativePaymentDueDays =
        Error.Validation("Customer.NegativePaymentDueDays", "Default payment due days cannot be negative.");

    public static readonly Error InvalidDiscountPercent =
        Error.Validation("Customer.InvalidDiscountPercent", "Early payment discount percent must be between 0 and 1.");

    public static readonly Error NegativeDiscountDays =
        Error.Validation("Customer.NegativeDiscountDays", "Early payment discount days cannot be negative.");

    public static readonly Error DiscountDaysRequiredWithPercent =
        Error.Validation("Customer.DiscountDaysRequiredWithPercent", "Early payment discount days are required when a discount percent is set.");

    public static readonly Error DiscountPercentRequiredWithDays =
        Error.Validation("Customer.DiscountPercentRequiredWithDays", "Early payment discount percent is required when discount days are set.");
}