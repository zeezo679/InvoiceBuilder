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

    public static Customer Create(string userId, string customerName, string companyName, string email, string billingAddressLine1,
        string billingCity, string billingState, string billingPostalCode, string billingCountry)
    {
        RequireNonEmpty(userId, nameof(userId));
        RequireNonEmpty(customerName, nameof(customerName));
        RequireNonEmpty(companyName, nameof(companyName));
        RequireValidEmail(email);
        RequireNonEmpty(billingAddressLine1, nameof(billingAddressLine1));
        RequireNonEmpty(billingCity, nameof(billingCity));
        RequireNonEmpty(billingState, nameof(billingState));
        RequireNonEmpty(billingPostalCode, nameof(billingPostalCode));
        RequireNonEmpty(billingCountry, nameof(billingCountry));

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

    public void UpdatePersonalInfo(string customerName, string companyName, string? contactName, string email, string? phone, string? website)
    {
        EnsureNotDeleted();

        RequireNonEmpty(customerName, nameof(customerName));
        RequireNonEmpty(companyName, nameof(companyName));
        RequireValidEmail(email);

        CustomerName = customerName.Trim();
        CompanyName = companyName.Trim();
        ContactName = contactName?.Trim();
        Email = email.Trim().ToLower();
        Phone = phone?.Trim();
        Website = website?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateBillingAddress(string billingAddressLine1, string? billingAddressLine2, string billingCity, string billingState, string billingPostalCode, string billingCountry)
    {
        EnsureNotDeleted();

        RequireNonEmpty(billingAddressLine1, nameof(billingAddressLine1));
        RequireNonEmpty(billingCity, nameof(billingCity));
        RequireNonEmpty(billingState, nameof(billingState));
        RequireNonEmpty(billingPostalCode, nameof(billingPostalCode));
        RequireNonEmpty(billingCountry, nameof(billingCountry));

        BillingAddressLine1 = billingAddressLine1.Trim();
        BillingAddressLine2 = billingAddressLine2?.Trim();
        BillingCity = billingCity.Trim();
        BillingState = billingState.Trim();
        BillingPostalCode = billingPostalCode.Trim();
        BillingCountry = billingCountry.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetShippingAddress(string? shippingAddressLine1, string? shippingAddressLine2, string? shippingCity, string? shippingState, string? shippingPostalCode, string? shippingCountry)
    {
        EnsureNotDeleted();

        if (!string.IsNullOrWhiteSpace(shippingAddressLine1))
        {
            RequireNonEmpty(shippingCity, nameof(shippingCity), "Shipping city is required when address is provided.");
            RequireNonEmpty(shippingState, nameof(shippingState), "Shipping state is required when address is provided.");
            RequireNonEmpty(shippingPostalCode, nameof(shippingPostalCode), "Shipping postal code is required when address is provided.");
            RequireNonEmpty(shippingCountry, nameof(shippingCountry), "Shipping country is required when address is provided.");
        }

        ShippingAddressLine1 = shippingAddressLine1?.Trim();
        ShippingAddressLine2 = shippingAddressLine2?.Trim();
        ShippingCity = shippingCity?.Trim();
        ShippingState = shippingState?.Trim();
        ShippingPostalCode = shippingPostalCode?.Trim();
        ShippingCountry = shippingCountry?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetTaxInfo(string? taxId, bool isTaxExempt, string? taxExemptionNumber, string? taxExemptionCertificateUrl, DateTime? taxExemptionExpiryDate)
    {
        EnsureNotDeleted();

        if (isTaxExempt)
        {
            RequireNonEmpty(taxExemptionNumber, nameof(taxExemptionNumber), "Tax exemption number is required for tax exempt customers.");
            RequireNonEmpty(taxExemptionCertificateUrl, nameof(taxExemptionCertificateUrl), "Tax exemption certificate URL is required for tax exempt customers.");
            
            if (!taxExemptionExpiryDate.HasValue)
                throw new ArgumentException("Tax exemption expiry date is required for tax exempt customers.", nameof(taxExemptionExpiryDate));
            if (taxExemptionExpiryDate.Value <= DateTime.UtcNow)
                throw new ArgumentException("Tax exemption expiry date must be in the future.", nameof(taxExemptionExpiryDate));
        }

        TaxId = taxId?.Trim();
        IsTaxExempt = isTaxExempt;
        TaxExemptionNumber = isTaxExempt ? taxExemptionNumber?.Trim() : null;
        TaxExemptionCertificateUrl = isTaxExempt ? taxExemptionCertificateUrl?.Trim() : null;
        TaxExemptionExpiryDate = isTaxExempt ? taxExemptionExpiryDate : null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPaymentTerms(int? defaultPaymentDueDays, decimal? earlyPaymentDiscountPercent, int? earlyPaymentDiscountDays)
    {
        EnsureNotDeleted();
        
        if (defaultPaymentDueDays.HasValue && defaultPaymentDueDays < 0)
            throw new ArgumentException("Default payment due days cannot be negative.", nameof(defaultPaymentDueDays));
        if (earlyPaymentDiscountPercent.HasValue && (earlyPaymentDiscountPercent < 0 || earlyPaymentDiscountPercent > 1))
            throw new ArgumentException("Early payment discount percent must be between 0 and 1.", nameof(earlyPaymentDiscountPercent));
        if (earlyPaymentDiscountDays.HasValue && earlyPaymentDiscountDays < 0)
            throw new ArgumentException("Early payment discount days cannot be negative.", nameof(earlyPaymentDiscountDays));
        if (earlyPaymentDiscountPercent.HasValue && !earlyPaymentDiscountDays.HasValue)
            throw new ArgumentException("Early payment discount days are required when a discount percent is set.", nameof(earlyPaymentDiscountDays));
        if (earlyPaymentDiscountDays.HasValue && !earlyPaymentDiscountPercent.HasValue)
            throw new ArgumentException("Early payment discount percent is required when discount days are set.", nameof(earlyPaymentDiscountPercent));

        DefaultPaymentDueDays = defaultPaymentDueDays;
        EarlyPaymentDiscountPercent = earlyPaymentDiscountPercent;
        EarlyPaymentDiscountDays = earlyPaymentDiscountDays;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        EnsureNotDeleted();

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        EnsureNotDeleted();

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
    
    
    // --- Helpers ---

    private static void RequireNonEmpty(string? value, string paramName, string? message = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(message ?? $"{paramName} is required.", paramName);
    }

    private static void RequireValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            if (addr.Address != email.Trim().ToLower())
                throw new ArgumentException("Email is invalid.", nameof(email));
        }
        catch (FormatException)
        {
            throw new ArgumentException("Email is invalid.", nameof(email));
        }
    }

    
    private void EnsureNotDeleted()
    {
        if (IsDeleted)
            throw new InvalidOperationException("Operation cannot be performed on a deleted customer.");
    }
    
}