using System;

namespace Basket.Service.Dtos
{
    public class BasketItemDto
    {
        public Guid BasketParentId { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public int QuanityOrdered { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public bool ParentalCaution { get; set; }
        public string Condition { get; set; }
        public string Status { get; set; }
        public string Medium { get; set; }
        public DateTime DateCreated { get; set; }
        public string Etag { get; set; }
        public bool HighValueItem { get; set; }
    }
}