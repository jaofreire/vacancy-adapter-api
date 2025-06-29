namespace CurriculumAdapter.API.DTOs
{
    public record Payment(
        string Object,
        string Id,
        string DateCreated,
        string Customer,
        string? Subscription,
        string? Installment,
        string? PaymentLink,
        string DueDate,
        string OriginalDueDate,
        decimal Value,
        decimal NetValue,
        decimal? OriginalValue,
        decimal? InterestValue,
        string? NossoNumero,
        string Description,
        string ExternalReference,
        string BillingType,
        string Status,
        object? PixTransaction,
        string? ConfirmedDate,
        string? PaymentDate,
        string? ClientPaymentDate,
        int? InstallmentNumber,
        string? CreditDate,
        object? Custody,
        string EstimatedCreditDate,
        string InvoiceUrl,
        string? BankSlipUrl,
        string? TransactionReceiptUrl,
        string InvoiceNumber,
        bool Deleted,
        bool Anticipated,
        bool Anticipable,
        string? LastInvoiceViewedDate,
        string? LastBankSlipViewedDate,
        bool PostalService,
        CreditCardWebHook? CreditCard,
        Discount Discount,
        Fine Fine,
        Interest Interest,
        List<Split>? Split,
        Chargeback? Chargeback,
        object? Refunds
    );

    public record CreditCardWebHook(
        string CreditCardNumber,
        string CreditCardBrand,
        string CreditCardToken
    );

    public record Discount(
        decimal Value,
        int DueDateLimitDays,
        string? LimitedDate,
        string Type
    );

    public record Fine(
        decimal Value,
        string Type
    );

    public record Interest(
        decimal Value,
        string Type
    );

    public record Split(
        string Id,
        string WalletId,
        decimal? FixedValue,
        decimal? PercentualValue,
        string Status,
        string? RefusalReason,
        string? ExternalReference,
        string? Description
    );

    public record Chargeback(
        string Status,
        string Reason
    );

    public record WebhookEventInputDTO
        (
        string Id,
        string Event,
        string DateCreated,
        Payment Payment
    );
    
}
