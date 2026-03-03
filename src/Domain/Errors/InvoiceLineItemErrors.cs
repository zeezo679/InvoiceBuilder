using ErrorOr;

namespace Domain.Errors;

public class InvoiceLineItemErrors
{
    public static readonly Error NotFound =
        Error.NotFound("InvoiceLineItem.NotFound", "The invoice line item was not found.");
    
    public static readonly Error InvoiceIdMismatch =
        Error.Conflict("InvoiceLineItem.InvoiceIdMismatch", "The line item does not belong to the specified invoice.");
    
    public static readonly Error InvalidDescription = 
        Error.Validation("InvoiceLineItem.InvalidDescription", "Description is required.");
    
    public static readonly Error InvalidQuantity = 
        Error.Validation("InvoiceLineItem.InvalidQuantity", "Quantity must be greater than zero.");
    
    public static readonly Error InvalidUnitPrice = 
        Error.Validation("InvoiceLineItem.InvalidUnitPrice", "Unit price must be greater than zero and less than or equal to the total amount.");
    
    public static readonly Error InvalidTaxRate = 
        Error.Validation("InvoiceLineItem.InvalidTaxRate", "Tax rate must be between 0 and 1.");
    
    public static readonly Error AlreadyRefunded = 
        Error.Conflict("InvoiceLineItem.AlreadyRefunded", "Line item is already refunded.");
    
    public static readonly Error RefundAmountExceedsTotalAmountOrLessThanZero = 
        Error.Validation("InvoiceLineItem.RefundAmountExceedsTotalAmountOrLessThanZero", "Refund amount must be greater than 0 and less than or equal to the total amount.");
}