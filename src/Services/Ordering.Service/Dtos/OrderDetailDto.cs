namespace Ordering.API.Dtos
{
    public class OrderDetailDto
    {
        public int OrderDetailId { get; set; }
        public string OrderId { get; set; }
        public int AlbumId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
    }
}