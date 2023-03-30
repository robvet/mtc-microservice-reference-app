using System;
using System.Collections.Generic;
using EventBus.Events;
using Ordering.API.Dtos;
using Ordering.Domain.AggregateModels.BuyerAggregate;
using Ordering.Domain.AggregateModels.OrderAggregate;

namespace Ordering.API.Events
{
    public class CheckOutEvent : MessageEvent
    {
        public OrderInformationModel OrderInformationModel { get; set; }

        //    public CheckOutEvent()
        //    {
        //        LineItems = new List<CheckOutEventLineItem>();
        //    }

        //    public string BasketId { get; set; }
        //    public string CheckoutId { get; set; }
        //    public decimal Total { get; set; }
        //    public CheckOutBuyer BuyerInformation { get; set; }
        //    public PaymentInformation PaymentInformation { get; set; }
        //    public List<CheckOutEventLineItem> LineItems { get; set; }
        //}

        //public class CheckOutEventLineItem
        //{
        //    public int ProductId { get; set; }
        //    public string Title { get; set; }
        //    public string Artist { get; set; }
        //    public string Genre { get; set; }
        //    public string UnitPrice { get; set; }
        //    public int Quantity { get; set; }
        //}

        //public class CheckOutBuyer
        //{
        //    public string Username { get; set; }
        //    public string FirstName { get; set; }
        //    public string LastName { get; set; }
        //    public string Address { get; set; }
        //    public string City { get; set; }
        //    public string State { get; set; }
        //    public string PostalCode { get; set; }
        //    public string Phone { get; set; }
        //    public string Email { get; set; }
        //}

        //public class PaymentInformation
        //{
        //    public string CreditCardNumber { get; set; }
        //    public string SecurityCode { get; set; }
        //    public string CardholderName { get; set; }
        //    public DateTime ExpirationDate { get; set; }
        //}
    }
}