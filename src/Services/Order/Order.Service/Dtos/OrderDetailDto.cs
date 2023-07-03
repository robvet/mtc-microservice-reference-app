using System;

namespace order.service.Dtos
{
    public class OrderDetailDto
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string Title { get; set; }
        public Guid ArtistId { get; set; }
        public string Artist { get; set; }
        public Guid GenreId { get; set; }
        public string Genre { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Condition { get; set; }
        public string Status { get; set; }
        public Guid MediumId { get; set; }
        public string Medium { get; set; }
        public DateTime DateCreated { get; set; }
        public bool BackOrdered { get; set; }
        public bool HighValueItem { get; set; }
    }
}