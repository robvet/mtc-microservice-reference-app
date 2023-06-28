using Newtonsoft.Json;

namespace order.domain.Models.ReadModels
{
    public class OrderDetailReadModel
    {
        //public int OrderDetailId { get; set; }
        //public int orderid { get; set; }
        public Guid ProductId { get; set; }
        public string Title { get; set; }
        public string ReleaseYear { get; set; }
        public bool HighValueItem { get; set; }
        public string UPC { get; set; }
        public string ArtistId { get; set; }
        public string Artist { get; set; }
        public string GenreId { get; set; }
        public string Genre { get; set; }
        public string MediumId { get; set; }
        public string Medium { get; set; }
        public string ConditionId { get; set; }
        public string Condition { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public bool BackOrdered { get; set; }
        [JsonIgnore]
        public OrderReadModel Order { get; set; }
    }
}