using ErrorOr;

namespace Domain.Errors.Payment;

public class PaymentErrors
{
    //InvoiceId must not be empty
    //Amount must not be no <= zero
    //RecordedByUserId must not be empty

    public static readonly Error EmptyInvoiceId =
        Error.Validation("Payment.EmptyInvoiceId", "Invoice Id is empty");

    public static readonly Error EmptyRecordedByUserId =
        Error.Validation("Payment.EmptyRecordedByUserId", "Empty Recorded By User Id");
    
    public static readonly Error EmptyRefundedByUserId =
        Error.Validation("Payment.EmptyRefundedByUserId", "Empty Refunded By User Id");
    
    public static readonly Error InvalidAmount =
        Error.Validation("Payment.InvalidAmount", "Paid amount must be greater than 0");

    public static readonly Error StatusAlreadyFailed =
        Error.Validation("Payment.StatusAlreadyFailed", "Cannot do a process on a Failed Payment");

    public static readonly Error StatusIsPending =
        Error.Validation("Payment.StatusIsPending", "Must pay to refund");

    public static readonly Error AlreadyRefunded =
        Error.Validation("Payment.AlreadyRefunded", "Payment already refunded");

}