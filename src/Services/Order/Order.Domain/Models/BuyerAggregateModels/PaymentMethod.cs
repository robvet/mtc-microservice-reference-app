﻿using System;
using System.Text.Json.Serialization;
using order.domain.Models.OrderAggregateModels;
using order.domain.Contracts;

namespace order.domain.Models.BuyerAggregateModels
{
    public class PaymentMethod : IAggregateRoot
    {
        public PaymentMethod()
        { }

        public PaymentMethod(
            string cardNumber,
            string securityCode,
            string cardHolderName,
            DateTime expirationDate)
        {
            CardNumber = !string.IsNullOrEmpty(cardNumber)
                ? cardNumber
                : throw new ArgumentNullException(cardNumber, "Credit Card number missing");
            SecurityCode = !string.IsNullOrEmpty(securityCode)
                ? securityCode
                : throw new ArgumentNullException(securityCode, "Credit Card number missing");
            CardHolderName = !string.IsNullOrEmpty(cardHolderName)
                ? cardHolderName
                : throw new ArgumentNullException(cardHolderName, "Credit Card number missing");
            ExpirationDate = expirationDate > DateTime.UtcNow
                ? expirationDate
                : throw new ArgumentNullException(expirationDate.ToShortDateString(), "Card is expired");
        }

        // DDD Patterns comment
        // Using private fields to encapsulate and carefully manage data.
        // The only way to create an PaymentMethod is through the constructor enabling
        // the domain class to enforce business rules and validation
        //public int Id { get; private set; }
        public string CardNumber { get; }
        public string SecurityCode { get; }
        public DateTime ExpirationDate { get; }
        public string CardHolderName { get; }

        ////[JsonIgnore]
        //public Order Order { get; private set; }
        ////public Buyer Buyer { get; private set; }
    }
}