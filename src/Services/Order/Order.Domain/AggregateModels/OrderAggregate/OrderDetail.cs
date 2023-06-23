﻿using System;
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
            Guid albumId,
            int quantity,
            decimal unitPrice)
        {
            if (quantity <= 0)
                BackOrdered = true;
            else
                BackOrdered = false;

            if (unitPrice < 0m)
                throw new Exceptions.OrderingDomainException("Price cannot be zero");

            Title = title;
            Artist = artist;
            AlbumId = albumId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        //public int OrderDetailId { get; set; }
        //public int orderid { get; set; }
        public int Id { get; private set; }
        public Guid AlbumId { get; private set; }
        public string Title { get; private set; }
        public string Artist { get; private set; }
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