using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace order.domain.AggregateModels.OrderAggregate
{
    public class OrderDetail
    {
        public OrderDetail()
        {
        }

        public OrderDetail(string title,
            string artist,
            Guid productId,
            int quantity,
            string unitPrice)
        {
            if (quantity <= 0)
                BackOrdered = true;
            else
                BackOrdered = false;

            var unitPriceDecimal = Convert.ToDecimal(unitPrice);

            if (unitPriceDecimal < 0m)
                throw new Exceptions.OrderingDomainException("Price cannot be zero in populating OrderDetail in Constructor");

            Title = title;
            Artist = artist;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPriceDecimal;
        }

        //public int OrderDetailId { get; set; }
        //public int orderid { get; set; }
        public Guid ProductId { get; private set; }
        public string Title { get; private set; }
        public string ReleaseYear { get; set; }
        public bool HighValueItem { get; set; }
        public string UPC { get; set; }
        public string ArtistId { get; set; }
        public string Artist { get; private set; }
        public string GenreId { get; set; }
        public string Genre { get; set; }
        public string MediumId { get; set; }
        public string Medium { get; set; }
        public string ConditionId { get; set; }
        public string Condition { get; set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public bool BackOrdered { get; private set; }
        [JsonIgnore]
        public Order Order { get; private set; }


        public List<OrderDetail> GetOrderDetailsForOrder(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}