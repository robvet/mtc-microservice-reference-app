using order.domain.AggregateModels.BuyerAggregate;
using order.domain.Contracts;

namespace order.domain.AggregateModels.OrderAggregate
{
    public class Order : Item, IAggregateRoot
    {
        public Order()
        { }

        public Order(
            string customerId,
            string basketId,
            string messageId,
            decimal total,
            string correlationToken)
        {
            OrderId = Guid.NewGuid().ToString();
            Id = Guid.NewGuid().ToString();
            OrderDate = DateTime.UtcNow;
            CustomerId = customerId;
            Total = total;  
            CorrelationToken = correlationToken;
            BasketId = basketId;
            EventBusMessageId = messageId;
            OrderDetails = new List<OrderDetail>();
                        
            // Set status to Pending
            OrderStatusId = (int)OrderStatusEnum.Pending;
        }

        // DDD Patterns comment
        // Using private fields to encapsulate and carefully manage data.
        // The only way to create a Buyer is through the constructor enabling
        // the domain class to enforce business rules and validation

        public string OrderId { get; private set; }

        //[JsonProperty("orderId")]
        //public int Id { get; private set; }
        public int OrderStatusId { get; private set; }
        //public string OrderId { get; private set; }
        // This is the orderid for Cosmos
        public string CustomerId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public decimal Total { get; private set; }
        public string CorrelationToken { get; private set; }
        public string BasketId { get; set; }
        public string EventBusMessageId { get; private set; }

        public List<OrderDetail> OrderDetails { get; private set; }
        public Buyer Buyer { get; set; }
        public OrderStatus OrderStatus { get; private set; }


        // DDD Patterns comment:
        // This Order AggregateRoot's method "AddOrderitem()" should be the only way to add Items to the Order. This centralized approach provides consistency to the entire aggregate.
        public void AddOrderItem(string title, Guid productId, string artist, int quantity, decimal unitPrice)
        {
            OrderDetails.Add(new OrderDetail(title, artist, productId, quantity, unitPrice));
        }

        // DDD Patterns comment:
        // Any behavior(discounts, etc.) and validations are controlled by the Aggregate Root
        // in order to maintain consistency across the whole Aggregate.
        // This Order AggregateRoot's method "GetLineItemCount" is the only way to obtain a count
        // of items for an Order.;   
        public int GetLineItemCount()
        {
            return OrderDetails.Count;
        }

        // This Order AggregateRoot's method "GetOrderTotalPrice" is the only way to obtain
        // the price of the order.  
        public decimal GetOrderTotalPrice()
        {
            return Total;
        }

        // This Order AggregateRoot's method "ApplyDiscount" is the only way to apply a discount
        // to the order.  
        public decimal ApplyDiscount(decimal discount)
        {
            return Total * discount;
        }

        // This Order AggregateRoot's method "ProductsBackOrdered" is the only way to identity the 
        // number of products backorderd for an order.  
        public int ProductsBackOrdered()
        {
            return OrderDetails.Count(x => x.BackOrdered);
        }

        // This Order AggregateRoot's method "GetUnitsForOrder" is the only way to return the number
        // of units for an order.  
        public int GetUnitsForOrder()
        {
            return OrderDetails.Sum(x => x.Quantity);
        }

        // This Order AggregateRoot's method "GetOrderDetailsForOrder" is the only way to return the number
        // of fetch line item details for an order.  
        public List<OrderDetail> GetOrderDetailsForOrder(int orderId)
        {
            throw new NotImplementedException();
        }

        // This Order AggregateRoot's method "GetMostExpensvieLineItemForOrder" is the only way to 
        // return the highest priced line item for an order
        public List<OrderDetail> GetMostExpensvieLineItemForOrder(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}