namespace Domain.Entities.Payment;

using Domain.Enums.Payment;
using Domain.Errors.Payment;
using ErrorOr;

public class Payment
{
    public Guid Id { get; private set; }
    public int SequenceNumber { get; private set; }

    // [NotMapped] - auto-generated display number e.g. PAY-2025-0001
    public string PaymentNumber => $"PAY-{PaymentDate.Year}-{SequenceNumber:D4}";
    
    public Guid InvoiceId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime PaymentDate { get; private set; }
    public DateTime RecordedDate { get; private set; }
    public PaymentMethod Method { get; private set; }
    public PaymentStatus Status { get; private set; }
    public string? ReferenceNumber { get; private set; }
    public string? Notes { get; private set; }
    
    public string RecordedByUserId { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public bool IsRefunded { get; private set; }
    public DateTime? RefundedDate { get; private set; }
    public string? RefundedByUserId { get; private set; }
    public string? RefundReason { get; private set; }

    private Payment() { }

    public static ErrorOr<Payment> Create(
        Guid invoiceId,
        decimal amount,
        DateTime paymentDate,
        PaymentMethod method,
        string recordedByUserId,
        string? referenceNumber = null,
        string? notes = null)
    {
        
        //InvoiceId must not be empty
        //Amount must not be no <= zero
        //RecordedByUserId must not be empty
        if (invoiceId == Guid.Empty) return PaymentErrors.EmptyInvoiceId;
        if (string.IsNullOrWhiteSpace(recordedByUserId)) return PaymentErrors.EmptyRecordedByUserId;
        if (amount <= 0) return PaymentErrors.InvalidAmount;
        
        return new Payment
        {
            Id = Guid.NewGuid(),
            InvoiceId = invoiceId,
            Amount = amount,
            PaymentDate = paymentDate,
            RecordedDate = DateTime.UtcNow,
            Method = method,
            Status = PaymentStatus.Pending,
            ReferenceNumber = referenceNumber,
            Notes = notes,
            RecordedByUserId = recordedByUserId,
            IsRefunded = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    public ErrorOr<Updated> MarkAsCleared()
    {
        if (Status == PaymentStatus.Failed) return PaymentErrors.StatusAlreadyFailed;
        
        Status = PaymentStatus.Cleared;
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }

    public void MarkAsFailed()
    {
        Status = PaymentStatus.Failed;
        UpdatedAt = DateTime.UtcNow;
    }

    public ErrorOr<Updated> Refund(string refundedByUserId, string? reason = null)
    {
        if (IsRefunded)
            return PaymentErrors.AlreadyRefunded;
        
        if (Status == PaymentStatus.Failed) 
            return PaymentErrors.StatusAlreadyFailed;
        
        if (Status == PaymentStatus.Pending) 
            return PaymentErrors.StatusIsPending;
        
        if (string.IsNullOrWhiteSpace(refundedByUserId))
            return PaymentErrors.EmptyRefundedByUserId;
       
        IsRefunded = true;
        Status = PaymentStatus.Refunded;
        RefundedDate = DateTime.UtcNow;
        RefundedByUserId = refundedByUserId;
        RefundReason = reason;
        UpdatedAt = DateTime.UtcNow;

        return Result.Updated;
    }
}