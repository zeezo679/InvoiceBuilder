using ErrorOr;

namespace Domain.Errors;

public static class InvoiceErrors
{
    public static readonly Error Deleted = 
        Error.Conflict("Invoice.Deleted", "The invoice has been deleted.");
    
    public static readonly Error InvalidDate = 
        Error.Validation("Invoice.InvalidDate", "The invoice date is required.");
    
    public static readonly Error InvalidCustomerId = 
        Error.Validation("Invoice.InvalidCustomerId", "The customer ID is required.");
    
    public static readonly Error InvalidSenderId =
        Error.Validation("Invoice.InvalidSenderId", "The sender ID is required.");
    
    public static readonly Error InvalidUserId =
        Error.Validation("Invoice.InvalidUserId", "The user ID is required.");
    
    public static readonly Error InvalidPaymentTerms =
        Error.Validation("Invoice.InvalidPaymentTerms", "Payment terms are required.");

    public static readonly Error InvalidInvoiceLineItem =
        Error.Validation("Invoice.InvalidInvoiceLineItem", "The line item returned is not found");
    
    public static readonly Error AlreadyPaid =
        Error.Validation("Invoice.AlreadyPaid", "The invoice is already fully paid");

    public static readonly Error AlreadySent =
        Error.Validation("Invoice.AlreadySent", "The invoice is already sent");
    
    public static readonly Error NotSentYet =
        Error.Validation("Invoice.NotSentYet", "Invoice must be sent");

    public static readonly Error NotYetOverDue =
        Error.Validation("Invoice.NotYetOverDue", "Cannot mark as overdue");

    public static readonly Error NoAmountDue =
        Error.Validation("Invoice.NoAmountDue", "No Amount Due");
    
    public static readonly Error Rejected =
        Error.Validation("Invoice.Rejected", "The invoice is rejected. cannot perform status update");

    public static readonly Error NoPaymentMade =
        Error.Validation("Invoice.NoPaymentMade", "No Payment Made");

    public static readonly Error PayedAmountExceedsRequiredAmount =
        Error.Validation("Invoice.PayedAmountExceedsRequiredAmount", "Payed Amount Exceeds Required Amount");

    public static readonly Error AmountMismatch =
        Error.Validation("Invoice.AmountMismatch", "Payed Amount doesnt match required Amount");
}