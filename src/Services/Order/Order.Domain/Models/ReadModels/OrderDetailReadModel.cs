using Newtonsoft.Json;

namespace order.domain.Models.ReadModels
{
    public class OrderDetailReadModel
    {
        //public int OrderDetailId { get; set; }
        //public int orderid { get; set; }
        public Guid ProductId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Condition { get; set; }
        public string Status { get; set; }
        public string Medium { get; set; }
        public DateTime DateCreated { get; set; }
        public bool BackOrdered { get; set; }
        public bool HighValueItem { get; set; }
        
        [JsonIgnore]
        public OrderReadModel Order { get; set; }
    }
}