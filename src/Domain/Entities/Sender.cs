using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Runtime.CompilerServices;

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

    public static Sender Create(string userId, string businessName, string email, string addressLine1, string city,
        string state, string postalCode, string country)
    {
        //user id must not be null or empty
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID is required.", nameof(userId));
        if (string.IsNullOrWhiteSpace(businessName))
            throw new ArgumentException("Business name is required.", nameof(businessName));
        if (!IsValidEmail(email))
            throw new ArgumentException("Email is invalid.", nameof(email));
        if (string.IsNullOrWhiteSpace(addressLine1))
            throw new ArgumentException("Address line 1 is required.", nameof(addressLine1));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City is required.", nameof(city));
        if (string.IsNullOrWhiteSpace(state))
            throw new ArgumentException("State is required.", nameof(state));
        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("Postal code is required.", nameof(postalCode));
        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country is required.", nameof(country));

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
    
    public void UpdateBusinessInfo(string businessName, string? legalName = null)
    {
        EnsureNotDeleted();
        
        if (string.IsNullOrWhiteSpace(businessName))
            throw new ArgumentException("Business name is required.", nameof(businessName));
        
        BusinessName = businessName.Trim();
        LegalName = legalName?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdateContactInfo(string email, string? phone = null, string? website = null)
    {
        EnsureNotDeleted();
        
        if (!IsValidEmail(email))
            throw new ArgumentException("Email is invalid.", nameof(email));
        
        Email = email.Trim().ToLower();
        Phone = phone?.Trim();
        Website = website?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdateAddress(string addressLine1, string? addressLine2, string city, string state, string postalCode, string country)
    {
        EnsureNotDeleted();
        
        if (string.IsNullOrWhiteSpace(addressLine1))
            throw new ArgumentException("Address line 1 is required.", nameof(addressLine1));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City is required.", nameof(city));
        if (string.IsNullOrWhiteSpace(state))
            throw new ArgumentException("State is required.", nameof(state));
        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("Postal code is required.", nameof(postalCode));
        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country is required.", nameof(country));

        AddressLine1 = addressLine1.Trim();
        AddressLine2 = addressLine2?.Trim();
        City = city.Trim();
        State = state.Trim();
        PostalCode = postalCode.Trim();
        Country = country.Trim();
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetTaxInfo(string taxId, string? registrationNumber = null)
    {
        EnsureNotDeleted();
        
        if (string.IsNullOrWhiteSpace(taxId))
            throw new ArgumentException("Tax ID is required.", nameof(taxId));
        
        TaxId = taxId.Trim();
        RegistrationNumber = registrationNumber?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdateBankDetails(string bankName, string bankAccountNumber, string bankRoutingNumber, string? iban = null, string? swift = null)
    {
        EnsureNotDeleted();
        
        if (string.IsNullOrWhiteSpace(bankName))
            throw new ArgumentException("Bank name is required.", nameof(bankName));
        if (string.IsNullOrWhiteSpace(bankAccountNumber))
            throw new ArgumentException("Bank account number is required.", nameof(bankAccountNumber));
        if (string.IsNullOrWhiteSpace(bankRoutingNumber))
            throw new ArgumentException("Bank routing number is required.", nameof(bankRoutingNumber));

        BankName = bankName.Trim();
        BankAccountNumber = bankAccountNumber.Trim();
        BankRoutingNumber = bankRoutingNumber.Trim();
        IBAN = iban?.Trim();
        SWIFT = swift?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void ClearBankDetails()
    {        
        EnsureNotDeleted();

        BankName = null;
        BankAccountNumber = null;
        BankRoutingNumber = null;
        IBAN = null;
        SWIFT = null;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdateBrandColor(string primaryColor)
    {
        EnsureNotDeleted();

        if (string.IsNullOrWhiteSpace(primaryColor))
            throw new ArgumentException("Primary color is required.", nameof(primaryColor));
        
        PrimaryColor = primaryColor.Trim();
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
    
    public void RemoveLogo()
    {
        EnsureNotDeleted();
        
        LogoUrl = null;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SoftDelete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
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
    
    private void EnsureNotDeleted()
    {
        if (IsDeleted)
            throw new InvalidOperationException("Operation cannot be performed on a deleted sender.");
    }
}