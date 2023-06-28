using Newtonsoft.Json;
using order.domain.Models.BuyerAggregateModels;
using order.domain.Models.OrderAggregateModels;

namespace order.domain.Models.ReadModels
{
    public class BuyerReadModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public Order Order { get; set; }
        //[JsonIgnore]
        //public PaymentMethod PaymentMethod { get; set; }
    }
}