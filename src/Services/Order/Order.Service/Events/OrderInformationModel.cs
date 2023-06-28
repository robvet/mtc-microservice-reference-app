using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace order.service.Events
{
    public class OrderInformationModel
    {
        public OrderInformationModel()
        {
            LineItems = new List<LineItem>();
        }

        public Guid BasketId { get; set; }
        public Guid CustomerId { get; set; }
        public string MessageId { get; set; }
        public decimal Total { get; set; }
        public BuyerInformation Buyer { get; set; }
        public PaymentInformation Payment { get; set; }
        public List<LineItem> LineItems { get; set; }

        public class LineItem
        {
            public Guid ProductId { get; set; }
            public string Title { get; set; }
            public string Artist { get; set; }
            public string Genre { get; set; }
            public string UnitPrice { get; set; }
            public int Quantity { get; set; }
        }

        public class BuyerInformation
        {
            public string Username { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PostalCode { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
        }

        public class PaymentInformation
        {
            public string CreditCardNumber { get; set; }
            public string SecurityCode { get; set; }
            public string CardholderName { get; set; }
            public DateTime ExpirationDate { get; set; }
            public string PaymentConfirmationId { get; set; }
        }
    }
}

