namespace CurriculumAdapter.API.DTOs
{
    public record Subscription(
       string Object,
       string Id,
       string DateCreated,
       string Customer,
       string? PaymentLink,
       string NextDueDate,
       decimal Value,
       string Cycle,
       string Description,
       string ExternalReference,
       string BillingType,
       string Status,
       bool Deleted,
       bool SendPaymentByPostalService,
       CreditCardWebHook? CreditCard,
       Discount Discount,
       Fine Fine,
       Interest Interest,
       List<Split>? Split
   );

    public record WebhookSubscriptionEventInputDTO
       (
        string Id,
        string Event,
        string DateCreated,
        Subscription Subscription
       );
    
}
