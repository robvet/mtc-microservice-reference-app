using order.domain.Models.OrderAggregateModels;
using System.Text.Json.Serialization;

namespace order.domain.Models.ReadModels
{
    public class PaymentMethodReadModel
    {
        public string CardNumber { get; set; }
        public string SecurityCode { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string CardHolderName { get; set; }

        [JsonIgnore]
        public Order Order { get; set; }
    }
}