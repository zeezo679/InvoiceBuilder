using Domain.Enums;
using Domain.Errors;
using ErrorOr;

namespace Domain.Entities;

public class Invoice
{
    public Guid Id { get; private set; }
    public int SequenceNumber { get; private set; } //configure in the database to auto-increment for each invoice created
    public string InvoiceNumber => $"INV-{InvoiceDate.Year}-{SequenceNumber:D4}"; //e.g. INV-2025-0001
    public Guid SenderId { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid CreateByUserId { get; private set; }
    public DateTime InvoiceDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime SentDate { get; private set; }
    public decimal SubTotalAmount { get; private set; }
    public decimal TotalTaxAmount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public decimal AmountPaid { get; private set; }
    public decimal AmountDue { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public string PaymentTerms { get; private set; } //e.g. "Net 30", "Due on Receipt"
    public string? Notes { get; private set; }

    public List<InvoiceLineItem> LineItems { get; private set; } = new List<InvoiceLineItem>();

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }


    private Invoice()
    {
    }

    // // 9. Fix Create() — DueDate is never set, call SetDueDate() during creation

    public static ErrorOr<Invoice> Create(Guid senderId, Guid customerId, Guid createdByUserId, DateTime invoiceDate,
        string paymentTerms, string? notes = null)
    {
        if (senderId == Guid.Empty) return InvoiceErrors.InvalidSenderId;
        if (customerId == Guid.Empty) return InvoiceErrors.InvalidCustomerId;
        if (createdByUserId == Guid.Empty) return InvoiceErrors.InvalidUserId;
        if (invoiceDate == default) return InvoiceErrors.InvalidDate;
        if (string.IsNullOrWhiteSpace(paymentTerms)) return InvoiceErrors.InvalidPaymentTerms;

        return new Invoice
        {
            Id = Guid.NewGuid(),
            SenderId = senderId,
            CustomerId = customerId,
            CreateByUserId = createdByUserId,
            InvoiceDate = invoiceDate,
            PaymentTerms = paymentTerms,
            Notes = notes,
            DueDate = invoiceDate,
            Status = InvoiceStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public ErrorOr<Updated> AddLineItem(string description, int quantity, decimal unitPrice, decimal taxRate)
    {
        if (IsDeleted)
            return InvoiceErrors.Deleted;
        
        var lineItemResult = InvoiceLineItem.Create(Id, description, quantity, unitPrice, taxRate);
        
        if(lineItemResult.IsError)
            return lineItemResult.Errors;
        
        LineItems.Add(lineItemResult.Value);
        CalculateAmounts();
        
        return Result.Updated;
    }
    
    public ErrorOr<Updated> RefundLineItem(Guid lineItemId, string refundedByUserId, string refundReason, decimal? partialAmount = null)
    {
        var lineItem = LineItems.FirstOrDefault(lineItem => lineItem.Id == lineItemId);
        
        if (lineItem is null)
            return InvoiceLineItemErrors.NotFound;
        
        if(lineItem.InvoiceId != Id)
            return InvoiceLineItemErrors.InvoiceIdMismatch;
        
        var refundResult = lineItem.Refund(refundedByUserId, refundReason, partialAmount);

        if (refundResult.IsError)
            return refundResult.Errors;
        
        CalculateAmounts();
        
        return Result.Updated;
    }
    
    public ErrorOr<Updated> UpdateInvoiceDate(DateTime newInvoiceDate)
    {
        if (IsDeleted) return InvoiceErrors.Deleted;
        if (newInvoiceDate == default) return InvoiceErrors.InvalidDate;

        InvoiceDate = newInvoiceDate;
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }
    
    public ErrorOr<Updated> UpdatePaymentTerms(string newPaymentTerms)
    {
        if (IsDeleted) return InvoiceErrors.Deleted;
        if (string.IsNullOrEmpty(newPaymentTerms)) return InvoiceErrors.InvalidPaymentTerms;

        PaymentTerms = newPaymentTerms;

        var result = SetDueDate(newPaymentTerms);
        if (result.IsError)
            return Error.Conflict("Invoice.DueDateError", "Failed to set due date");

        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }
    

    private ErrorOr<Updated> SetDueDate(string paymentTerms)
    {
        if (IsDeleted) return InvoiceErrors.Deleted;

        int days = 0; // default: on receipt

        if (!paymentTerms.Equals("On Receipt", StringComparison.OrdinalIgnoreCase))
        {
            var parts = paymentTerms.Split(' ');
            if (parts.Length < 2 || !int.TryParse(parts.Last(), out days) || days <= 0)
                return InvoiceErrors.InvalidPaymentTerms;
        }

        DueDate   = InvoiceDate.AddDays(days);
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }
    
    private ErrorOr<Updated> CalculateAmounts()
    {
        if (IsDeleted) return InvoiceErrors.Deleted;
        
        SubTotalAmount = LineItems.Sum(lineItem => lineItem.Quantity * lineItem.UnitPrice);
        TotalTaxAmount = LineItems.Sum(lineItem => lineItem.Quantity * lineItem.UnitPrice * lineItem.TaxRate);
        TotalAmount = SubTotalAmount + TotalTaxAmount;
        AmountDue = TotalAmount - AmountPaid;

        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }
    
    public ErrorOr<Updated> MarkAsSent()
    {
        if (IsDeleted) return InvoiceErrors.Deleted;
        if (Status == InvoiceStatus.Sent) return InvoiceErrors.AlreadySent;
        if (Status == InvoiceStatus.Paid) return InvoiceErrors.AlreadyPaid;
        if (Status == InvoiceStatus.Rejected) return InvoiceErrors.Rejected;

        Status = InvoiceStatus.Sent;
        SentDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;

    }
    
    public ErrorOr<Updated> MarkAsDue()
    {
        if (IsDeleted) return InvoiceErrors.Deleted;
        if (Status != InvoiceStatus.Sent) return InvoiceErrors.NotSentYet;

        Status = InvoiceStatus.Due;
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }
    
    public ErrorOr<Updated> MarkAsOverdue()
    {
        if (IsDeleted) return InvoiceErrors.Deleted;
        if (Status == InvoiceStatus.Paid) return InvoiceErrors.AlreadyPaid;
        if (Status == InvoiceStatus.Rejected) return InvoiceErrors.Rejected;
        if (Status != InvoiceStatus.Sent) return InvoiceErrors.NotSentYet;
        if (DueDate >= DateTime.UtcNow) return InvoiceErrors.NotYetOverDue;
        if (AmountDue == 0) return InvoiceErrors.NoAmountDue;

        Status = InvoiceStatus.Overdue;
        UpdatedAt = DateTime.UtcNow;
        return Result.Updated;
    
    }
    
    public ErrorOr<Updated> MarkAsPartiallyPaid()
    {
        if (IsDeleted) return InvoiceErrors.Deleted;
        if (Status == InvoiceStatus.Paid) return InvoiceErrors.AlreadyPaid;
        if (Status == InvoiceStatus.Rejected) return InvoiceErrors.Rejected;
        if (AmountPaid <= 0) return InvoiceErrors.NoPaymentMade;
        if (AmountPaid == TotalAmount) return InvoiceErrors.AlreadyPaid;
        if (AmountPaid > TotalAmount) return InvoiceErrors.PayedAmountExceedsRequiredAmount;
        
        
        Status = InvoiceStatus.PartiallyPaid;
        UpdatedAt = DateTime.UtcNow;
        
        return Result.Updated;

    }

    public ErrorOr<Updated> MarkAsPaid()
    {
        //an invoice is considered paid when AmountPaid == TotalAmount
        //and only when the invoice is not deleted or rejected and ofc if it is not paid before
        if (IsDeleted) return InvoiceErrors.Deleted;
        if (Status == InvoiceStatus.Paid) return InvoiceErrors.AlreadyPaid;
        if (Status == InvoiceStatus.Rejected) return InvoiceErrors.Rejected;
        if (AmountPaid <= 0) return InvoiceErrors.NoPaymentMade;
        if (AmountPaid != TotalAmount) return InvoiceErrors.AmountMismatch;
        
        Status = InvoiceStatus.Paid;
        UpdatedAt = DateTime.UtcNow;
        return Result.Updated;
    }

    public ErrorOr<Updated> MarkAsRejected()
    {
        if (IsDeleted) return InvoiceErrors.Deleted;
        if (Status == InvoiceStatus.Paid) return InvoiceErrors.AlreadyPaid;
        if (Status == InvoiceStatus.Rejected) return InvoiceErrors.Rejected;

        Status = InvoiceStatus.Rejected;
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }

    public ErrorOr<Deleted> SoftDelete()
    {
        if (IsDeleted) return InvoiceErrors.Deleted; //Already Deleted
    
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        return Result.Deleted;
    }

    public ErrorOr<Updated> RecordPayment(decimal amount)
    {
        if (amount <= 0) return InvoiceErrors.NoPaymentMade;
        if (amount > AmountDue) return InvoiceErrors.PayedAmountExceedsRequiredAmount;
        
        AmountPaid += amount;
        AmountDue -= amount;

        if (AmountDue == 0)
            MarkAsPaid();
        else 
            MarkAsPartiallyPaid();

        return Result.Updated;
    }

    public ErrorOr<Deleted> RemoveLineItem(Guid lineItemId)
    {
        var lineItem = LineItems.FirstOrDefault(lineItem => lineItem.Id == lineItemId);

        if (lineItem is null) return InvoiceErrors.InvalidInvoiceLineItem;
        if (lineItem.IsDeleted) return Result.Deleted;

        lineItem.IsDeleted = true;
        
        return Result.Deleted;
    }
    
}


// TODO - Done
