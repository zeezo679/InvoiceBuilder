using Domain.Errors;
using ErrorOr;

namespace Domain.Entities;

public class InvoiceLineItem
{
    public Guid Id { get; private set; }
    public Guid InvoiceId { get; private set; }
    public string Description { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TaxRate { get; private set; }
    public bool IsRefunded { get; private set; } = false;
    public DateTime? RefundedDate { get; private set; }
    public string? RefundedByUserId { get; private set; }
    public string? RefundReason { get; private set; }
    public decimal RefundedAmount { get; private set; }
    
    //Amounts
    public decimal Amount { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal TotalAmount { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsDeleted { get; set; }
    
    public Invoice Invoice { get; set; }  //navigation property
    
    private InvoiceLineItem(){}
    
    //factory method
    public static ErrorOr<InvoiceLineItem> Create(Guid invoiceId, string description, int quantity, decimal unitPrice, decimal taxRate)
    {
        var validationResult = ValidateLineItemCreation(description, quantity, unitPrice, taxRate);
        
        if(validationResult.Count > 0)
            return validationResult;
        
        return new InvoiceLineItem
        {
            InvoiceId = invoiceId,
            Description = description,
            Quantity = quantity,
            UnitPrice = unitPrice,
            TaxRate = taxRate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

    }
    
    public ErrorOr<Updated> Refund(string refundedByUserId, string refundReason, decimal? partialAmount = null)
    {
   
        if (IsRefunded)
            return InvoiceLineItemErrors.AlreadyRefunded;
        
        var refundAmount = partialAmount ?? TotalAmount;
        
        RefundedAmount += refundAmount; 
        
        if(RefundedAmount > TotalAmount || RefundedAmount <= 0)
            return InvoiceLineItemErrors.RefundAmountExceedsTotalAmountOrLessThanZero;
        
        IsRefunded = true;
        RefundedDate = DateTime.UtcNow;
        RefundedByUserId = refundedByUserId;
        RefundReason = refundReason;
        
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }
    
    public void CalculateAmounts()
    {
        Amount = Quantity * UnitPrice;
        TaxAmount = Amount * TaxRate;
        TotalAmount = Amount + TaxAmount;
        
        UpdatedAt = DateTime.UtcNow;
    }
    
    
    
    // Validators
    private static List<Error> ValidateLineItemCreation(string description,int quantity, decimal unitPrice, decimal taxRate)
    {
        List<Error> errors = [];
        
        if(string.IsNullOrWhiteSpace(description))
            errors.Add(InvoiceLineItemErrors.InvalidDescription);
        if (quantity <= 0)
            errors.Add(InvoiceLineItemErrors.InvalidQuantity);
        if (unitPrice <= 0)
            errors.Add(InvoiceLineItemErrors.InvalidUnitPrice);
        if (taxRate < 0 || taxRate > 1)
            errors.Add(InvoiceLineItemErrors.InvalidTaxRate);
        
        return errors;
    }
    
}