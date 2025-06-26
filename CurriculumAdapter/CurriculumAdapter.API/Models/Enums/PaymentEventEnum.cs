namespace CurriculumAdapter.API.Models.Enums
{
    public enum PaymentEventEnum
    {
        PAYMENT_CREATED = 0,
        PAYMENT_CONFIRMED = 1,
        PAYMENT_RECEIVED = 2,
        PAYMENT_ANTICIPATED = 3,
        PAYMENT_DELETED = 4,
        PAYMENT_REFUNDED = 5,
        PAYMENT_OVERDUE = 6
    }
}
