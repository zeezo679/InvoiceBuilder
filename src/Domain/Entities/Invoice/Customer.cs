using Domain.Errors;
using ErrorOr;

namespace Domain.Entities;

public class Customer
{
    public Guid Id { get; private set; }
    public string UserId { get; private set; } = null!;

    public string CustomerName { get; private set; } = null!;
    public string CompanyName { get; private set; } = null!;
    public string? ContactName { get; private set; }
    public string Email { get; private set; } = null!;
    public string? Phone { get; private set; }
    public string? Website { get; private set; }

    public string BillingAddressLine1 { get; private set; } = null!;
    public string? BillingAddressLine2 { get; private set; }
    public string BillingCity { get; private set; } = null!;
    public string BillingState { get; private set; } = null!;
    public string BillingPostalCode { get; private set; } = null!;
    public string BillingCountry { get; private set; } = null!;

    public string? ShippingAddressLine1 { get; private set; }
    public string? ShippingAddressLine2 { get; private set; }
    public string? ShippingCity { get; private set; }
    public string? ShippingState { get; private set; }
    public string? ShippingPostalCode { get; private set; }
    public string? ShippingCountry { get; private set; }

    public string? TaxId { get; private set; }
    public bool IsTaxExempt { get; private set; }
    public string? TaxExemptionNumber { get; private set; }
    public string? TaxExemptionCertificateUrl { get; private set; }
    public DateTime? TaxExemptionExpiryDate { get; private set; }
    
    public int? DefaultPaymentDueDays { get; private set; } = 30;
    public decimal? EarlyPaymentDiscountPercent { get; private set; }
    public int? EarlyPaymentDiscountDays { get; private set; }

    public string DefaultPaymentTerms =>
        DefaultPaymentDueDays switch
        {
            null => "No terms set",
            0 => "Due on receipt",
            _ => EarlyPaymentDiscountPercent.HasValue
                ? $"{EarlyPaymentDiscountPercent * 100:0.##}/{EarlyPaymentDiscountDays} Net {DefaultPaymentDueDays}"
                : $"Net {DefaultPaymentDueDays}"
        };


    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    private Customer(){}

    public static ErrorOr<Customer> Create(string userId, string customerName, string companyName, string email, string billingAddressLine1,
        string billingCity, string billingState, string billingPostalCode, string billingCountry)
    {
        if (string.IsNullOrWhiteSpace(userId)) return CustomerErrors.EmptyUserId;
        if (string.IsNullOrWhiteSpace(customerName)) return CustomerErrors.EmptyCustomerName;
        if (string.IsNullOrWhiteSpace(companyName)) return CustomerErrors.EmptyCompanyName;

        var emailError = ValidateEmail(email);
        if (emailError is not null) return emailError.Value;

        if (string.IsNullOrWhiteSpace(billingAddressLine1)) return CustomerErrors.EmptyBillingAddressLine1;
        if (string.IsNullOrWhiteSpace(billingCity)) return CustomerErrors.EmptyBillingCity;
        if (string.IsNullOrWhiteSpace(billingState)) return CustomerErrors.EmptyBillingState;
        if (string.IsNullOrWhiteSpace(billingPostalCode)) return CustomerErrors.EmptyBillingPostalCode;
        if (string.IsNullOrWhiteSpace(billingCountry)) return CustomerErrors.EmptyBillingCountry;

        return new Customer
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CustomerName = customerName.Trim(),
            CompanyName = companyName.Trim(),
            Email = email.Trim().ToLower(),
            BillingAddressLine1 = billingAddressLine1.Trim(),
            BillingCity = billingCity.Trim(),
            BillingState = billingState.Trim(),
            BillingPostalCode = billingPostalCode.Trim(),
            BillingCountry = billingCountry.Trim(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true,
            IsDeleted = false
        };
    }

    public ErrorOr<Updated> UpdatePersonalInfo(string customerName, string companyName, string? contactName, string email, string? phone, string? website)
    {
        if (IsDeleted) return CustomerErrors.Deleted;

        if (string.IsNullOrWhiteSpace(customerName)) return CustomerErrors.EmptyCustomerName;
        if (string.IsNullOrWhiteSpace(companyName)) return CustomerErrors.EmptyCompanyName;

        var emailError = ValidateEmail(email);
        if (emailError is not null) return emailError.Value;

        CustomerName = customerName.Trim();
        CompanyName = companyName.Trim();
        ContactName = contactName?.Trim();
        Email = email.Trim().ToLower();
        Phone = phone?.Trim();
        Website = website?.Trim();
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }

    public ErrorOr<Updated> UpdateBillingAddress(string billingAddressLine1, string? billingAddressLine2, string billingCity, string billingState, string billingPostalCode, string billingCountry)
    {
        if (IsDeleted) return CustomerErrors.Deleted;

        if (string.IsNullOrWhiteSpace(billingAddressLine1)) return CustomerErrors.EmptyBillingAddressLine1;
        if (string.IsNullOrWhiteSpace(billingCity)) return CustomerErrors.EmptyBillingCity;
        if (string.IsNullOrWhiteSpace(billingState)) return CustomerErrors.EmptyBillingState;
        if (string.IsNullOrWhiteSpace(billingPostalCode)) return CustomerErrors.EmptyBillingPostalCode;
        if (string.IsNullOrWhiteSpace(billingCountry)) return CustomerErrors.EmptyBillingCountry;

        BillingAddressLine1 = billingAddressLine1.Trim();
        BillingAddressLine2 = billingAddressLine2?.Trim();
        BillingCity = billingCity.Trim();
        BillingState = billingState.Trim();
        BillingPostalCode = billingPostalCode.Trim();
        BillingCountry = billingCountry.Trim();
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }

    public ErrorOr<Updated> SetShippingAddress(string? shippingAddressLine1, string? shippingAddressLine2, string? shippingCity, string? shippingState, string? shippingPostalCode, string? shippingCountry)
    {
        if (IsDeleted) return CustomerErrors.Deleted;

        if (!string.IsNullOrWhiteSpace(shippingAddressLine1))
        {
            if (string.IsNullOrWhiteSpace(shippingCity)) return CustomerErrors.EmptyShippingCity;
            if (string.IsNullOrWhiteSpace(shippingState)) return CustomerErrors.EmptyShippingState;
            if (string.IsNullOrWhiteSpace(shippingPostalCode)) return CustomerErrors.EmptyShippingPostalCode;
            if (string.IsNullOrWhiteSpace(shippingCountry)) return CustomerErrors.EmptyShippingCountry;
        }

        ShippingAddressLine1 = shippingAddressLine1?.Trim();
        ShippingAddressLine2 = shippingAddressLine2?.Trim();
        ShippingCity = shippingCity?.Trim();
        ShippingState = shippingState?.Trim();
        ShippingPostalCode = shippingPostalCode?.Trim();
        ShippingCountry = shippingCountry?.Trim();
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }

    public ErrorOr<Updated> SetTaxInfo(string? taxId, bool isTaxExempt, string? taxExemptionNumber, string? taxExemptionCertificateUrl, DateTime? taxExemptionExpiryDate)
    {
        if (IsDeleted) return CustomerErrors.Deleted;

        if (isTaxExempt)
        {
            if (string.IsNullOrWhiteSpace(taxExemptionNumber)) return CustomerErrors.EmptyTaxExemptionNumber;
            if (string.IsNullOrWhiteSpace(taxExemptionCertificateUrl)) return CustomerErrors.EmptyTaxExemptionCertificateUrl;
            if (!taxExemptionExpiryDate.HasValue) return CustomerErrors.MissingTaxExemptionExpiryDate;
            if (taxExemptionExpiryDate.Value <= DateTime.UtcNow) return CustomerErrors.ExpiredTaxExemption;
        }

        TaxId = taxId?.Trim();
        IsTaxExempt = isTaxExempt;
        TaxExemptionNumber = isTaxExempt ? taxExemptionNumber?.Trim() : null;
        TaxExemptionCertificateUrl = isTaxExempt ? taxExemptionCertificateUrl?.Trim() : null;
        TaxExemptionExpiryDate = isTaxExempt ? taxExemptionExpiryDate : null;
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }

    public ErrorOr<Updated> SetPaymentTerms(int? defaultPaymentDueDays, decimal? earlyPaymentDiscountPercent, int? earlyPaymentDiscountDays)
    {
        if (IsDeleted) return CustomerErrors.Deleted;

        if (defaultPaymentDueDays.HasValue && defaultPaymentDueDays < 0)
            return CustomerErrors.NegativePaymentDueDays;
        
        if (earlyPaymentDiscountPercent.HasValue && (earlyPaymentDiscountPercent < 0 || earlyPaymentDiscountPercent > 1))
            return CustomerErrors.InvalidDiscountPercent;
        
        if (earlyPaymentDiscountDays.HasValue && earlyPaymentDiscountDays < 0)
            return CustomerErrors.NegativeDiscountDays;
        
        if (earlyPaymentDiscountPercent.HasValue && !earlyPaymentDiscountDays.HasValue)
            return CustomerErrors.DiscountDaysRequiredWithPercent;
        
        if (earlyPaymentDiscountDays.HasValue && !earlyPaymentDiscountPercent.HasValue)
            return CustomerErrors.DiscountPercentRequiredWithDays;

        DefaultPaymentDueDays = defaultPaymentDueDays;
        EarlyPaymentDiscountPercent = earlyPaymentDiscountPercent;
        EarlyPaymentDiscountDays = earlyPaymentDiscountDays;
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }

    public ErrorOr<Updated> Activate()
    {
        if (IsDeleted) return CustomerErrors.Deleted;

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }

    public ErrorOr<Updated> Deactivate()
    {
        if (IsDeleted) return CustomerErrors.Deleted;

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }

    public ErrorOr<Deleted> SoftDelete()
    {
        if (IsDeleted) return CustomerErrors.Deleted;

        IsDeleted = true;
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;

        return Result.Deleted;
    }
    
    
    // --- Helpers ---

    private static Error? ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return CustomerErrors.EmptyEmail;
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            if (addr.Address != email.Trim().ToLower())
                return CustomerErrors.InvalidEmail;
        }
        catch (FormatException)
        {
            return CustomerErrors.InvalidEmail;
        }

        return null;
    }
    
}