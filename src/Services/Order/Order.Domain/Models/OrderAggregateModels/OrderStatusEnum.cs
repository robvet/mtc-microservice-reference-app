namespace order.domain.Models.OrderAggregateModels
{
    public enum OrderStatusEnum
    {
        Pending = 1,
        AwaitingValidation = 2,
        StockConfirmed = 3,
        Paid = 4,
        Shipped = 5,
        Complete = 6,
        Cancelled = 7
    }
}
