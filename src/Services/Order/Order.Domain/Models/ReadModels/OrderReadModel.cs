namespace order.domain.Models.ReadModels
{
    public class OrderReadModel : Item
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public int OrderStatusId { get; set; }
        public Guid BasketId { get; set; }
        public string EventBusMessageId { get; set; }
        public string CorrelationToken { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }

        public List<OrderDetailReadModel> OrderDetail { get; set; }
        public BuyerReadModel Buyer { get; set; }
        public OrderStatusReadModel OrderStatus { get; set; }
        public PaymentMethodReadModel PaymentMethod { get; set; }
    }
}