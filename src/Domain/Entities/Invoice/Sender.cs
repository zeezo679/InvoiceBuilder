using System.Net.Mail;
using Domain.Errors;
using ErrorOr;

namespace Domain.Entities;

public class Sender
{
    public Guid Id { get; private set; }
    public string UserId { get; private set; }
    
    public string BusinessName { get; private set; }
    public string? LegalName { get; private set; }
    
    public string Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Website { get; private set; }
    
    public string AddressLine1 { get; private set; }
    public string? AddressLine2 { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string PostalCode { get; private set; }
    public string Country { get; private set; }
    
    public string? TaxId { get; private set; }
    public string? RegistrationNumber { get; private set; }
    
    public string? LogoUrl { get; private set; }
    public string? PrimaryColor { get; private set; }   //personal brand color for the pdf styling
    
    public string? BankName { get; private set; }
    public string? BankAccountNumber { get; private set; }
    public string? BankRoutingNumber { get; private set; }
    public string? IBAN { get; private set; }
    public string? SWIFT { get; private set; }
    
    
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; } 
    
    private Sender(){}

    public static ErrorOr<Sender> Create(string userId, string businessName, string email, string addressLine1, string city,
        string state, string postalCode, string country)
    {
        if (string.IsNullOrWhiteSpace(userId)) return SenderErrors.EmptyUserId;
        if (string.IsNullOrWhiteSpace(businessName)) return SenderErrors.EmptyBusinessName;
        if (!IsValidEmail(email)) return SenderErrors.InvalidEmail;
        if (string.IsNullOrWhiteSpace(addressLine1)) return SenderErrors.EmptyAddressLine1;
        if (string.IsNullOrWhiteSpace(city)) return SenderErrors.EmptyCity;
        if (string.IsNullOrWhiteSpace(state)) return SenderErrors.EmptyState;
        if (string.IsNullOrWhiteSpace(postalCode)) return SenderErrors.EmptyPostalCode;
        if (string.IsNullOrWhiteSpace(country)) return SenderErrors.EmptyCountry;

        return new Sender
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            BusinessName = businessName.Trim(),
            Email = email.Trim().ToLower(),
            AddressLine1 = addressLine1.Trim(),
            City = city.Trim(),
            State = state.Trim(),
            PostalCode = postalCode.Trim(),
            Country = country.Trim(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true,
            IsDeleted = false
        };
    }
    
    public ErrorOr<Updated> UpdateBusinessInfo(string businessName, string? legalName = null)
    {
        if (IsDeleted) return SenderErrors.Deleted;
        if (string.IsNullOrWhiteSpace(businessName)) return SenderErrors.EmptyBusinessName;
        
        BusinessName = businessName.Trim();
        LegalName = legalName?.Trim();
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }
    
    public ErrorOr<Updated> UpdateContactInfo(string email, string? phone = null, string? website = null)
    {
        if (IsDeleted) return SenderErrors.Deleted;
        if (!IsValidEmail(email)) return SenderErrors.InvalidEmail;
        
        Email = email.Trim().ToLower();
        Phone = phone?.Trim();
        Website = website?.Trim();
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }
    
    public ErrorOr<Updated> UpdateAddress(string addressLine1, string? addressLine2, string city, string state, string postalCode, string country)
    {
        if (IsDeleted) return SenderErrors.Deleted;
        if (string.IsNullOrWhiteSpace(addressLine1)) return SenderErrors.EmptyAddressLine1;
        if (string.IsNullOrWhiteSpace(city)) return SenderErrors.EmptyCity;
        if (string.IsNullOrWhiteSpace(state)) return SenderErrors.EmptyState;
        if (string.IsNullOrWhiteSpace(postalCode)) return SenderErrors.EmptyPostalCode;
        if (string.IsNullOrWhiteSpace(country)) return SenderErrors.EmptyCountry;

        AddressLine1 = addressLine1.Trim();
        AddressLine2 = addressLine2?.Trim();
        City = city.Trim();
        State = state.Trim();
        PostalCode = postalCode.Trim();
        Country = country.Trim();
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }
    
    public ErrorOr<Updated> SetTaxInfo(string taxId, string? registrationNumber = null)
    {
        if (IsDeleted) return SenderErrors.Deleted;
        if (string.IsNullOrWhiteSpace(taxId)) return SenderErrors.EmptyTaxId;
        
        TaxId = taxId.Trim();
        RegistrationNumber = registrationNumber?.Trim();
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }
    
    public ErrorOr<Updated> UpdateBankDetails(string bankName, string bankAccountNumber, string bankRoutingNumber, string? iban = null, string? swift = null)
    {
        if (IsDeleted) return SenderErrors.Deleted;
        if (string.IsNullOrWhiteSpace(bankName)) return SenderErrors.EmptyBankName;
        if (string.IsNullOrWhiteSpace(bankAccountNumber)) return SenderErrors.EmptyBankAccountNumber;
        if (string.IsNullOrWhiteSpace(bankRoutingNumber)) return SenderErrors.EmptyBankRoutingNumber;

        BankName = bankName.Trim();
        BankAccountNumber = bankAccountNumber.Trim();
        BankRoutingNumber = bankRoutingNumber.Trim();
        IBAN = iban?.Trim();
        SWIFT = swift?.Trim();
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }
    
    public ErrorOr<Updated> ClearBankDetails()
    {        
        if (IsDeleted) return SenderErrors.Deleted;

        BankName = null;
        BankAccountNumber = null;
        BankRoutingNumber = null;
        IBAN = null;
        SWIFT = null;
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }
    
    public ErrorOr<Updated> UpdateBrandColor(string primaryColor)
    {
        if (IsDeleted) return SenderErrors.Deleted;
        if (string.IsNullOrWhiteSpace(primaryColor)) return SenderErrors.EmptyPrimaryColor;
        
        PrimaryColor = primaryColor.Trim();
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }
    
    public ErrorOr<Updated> Activate()
    {
        if (IsDeleted) return SenderErrors.Deleted;

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }
    
    public ErrorOr<Updated> Deactivate()
    {
        if (IsDeleted) return SenderErrors.Deleted;

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }
    
    public ErrorOr<Updated> RemoveLogo()
    {
        if (IsDeleted) return SenderErrors.Deleted;
        
        LogoUrl = null;
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }
    
    public ErrorOr<Deleted> SoftDelete()
    {
        if (IsDeleted) return SenderErrors.Deleted;
        
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;

        return Result.Deleted;
    }
    
    //Helper Methods
    private static bool IsValidEmail(string email)
    {
        bool valid = true;
        try
        {
            var mailAddress = new MailAddress(email);
        }
        catch
        {
            valid = false;
        }
        return valid;
    }
}