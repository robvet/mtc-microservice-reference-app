using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace order.domain.Models.OrderAggregateModels
{
    public class OrderDetail
    {
        public OrderDetail()
        { }

        public OrderDetail(Guid productId,
                           string title,
                           Guid artistId,
                           string artist,
                           Guid genreId,
                           string genre,
                           string unitPrice,
                           int quantity,
                           string condition,
                           string status,
                           Guid mediumId,
                           string medium,
                           DateTime dateCreated,
                           bool highValueItem)
        {
            // Set business rule for backordered merchandise
            if (quantity <= 0)
                BackOrdered = true;
            else
                BackOrdered = false;

            // Set business rule for unit price
            var unitPriceDecimal = Convert.ToDecimal(unitPrice);

            if (unitPriceDecimal < 0m)
                throw new Exceptions.OrderingDomainException("Price cannot be zero in populating OrderDetail in Constructor");

            ProductId = productId;
            Title = title;
            ArtistId = artistId;
            Artist = artist;
            GenreId = genreId;
            Genre = genre;
            UnitPrice = unitPriceDecimal;
            Quantity = quantity;
            Condition = condition;
            Status = status;
            MediumId = mediumId;
            Medium = medium;
            DateCreated = dateCreated;
            HighValueItem = highValueItem;
        }

        public Guid ProductId { get; set; }
        public string Title { get; private set; }
        public Guid ArtistId { get; private set; }
        public string Artist { get; private set; }
        public Guid GenreId { get; private set; }
        public string Genre { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }
        public string Condition { get; private set; }
        public string Status { get; private set; }
        public Guid MediumId { get; private set; }
        public string Medium { get; private set; }
        public DateTime DateCreated { get; private set; }
        public bool BackOrdered { get; private set; }
        public bool HighValueItem { get; private set; }
        [JsonIgnore]
        public Order Order { get; private set; }



        ////public int OrderDetailId { get; set; }
        ////public int orderid { get; set; }
        //public Guid ProductId { get; private set; }
        //public string Title { get; private set; }
        //public string ReleaseYear { get; set; }
        //public bool HighValueItem { get; set; }
        //public string UPC { get; set; }
        //public string ArtistId { get; set; }
        //public string Artist { get; private set; }
        //public string GenreId { get; set; }
        //public string Genre { get; set; }
        //public string MediumId { get; set; }
        //public string Medium { get; set; }
        //public string ConditionId { get; set; }
        //public string Condition { get; set; }
        //public int Quantity { get; private set; }
        //public decimal UnitPrice { get; private set; }
        //public bool BackOrdered { get; private set; }
        //[JsonIgnore]
        //public Order Order { get; private set; }


        public List<OrderDetail> GetOrderDetailsForOrder(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}