using System;

namespace order.service.Dtos
{
    public class OrderDetailDto
    {
        public Guid OrderId { get; set; }
        public Guid AlbumId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
    }
}