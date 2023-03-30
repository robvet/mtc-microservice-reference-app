using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Ordering.Domain.AggregateModels.OrderAggregate;
using Ordering.Domain.Contracts;

namespace Ordering.Domain.AggregateModels.BuyerAggregate
{
    public class Buyer : IAggregateRoot
    {
        public Buyer()
        {}

        public Buyer(
            string userName,
            string firstName,
            string lastName,
            string address,
            string city,
            string state,
            string postalCode,
            string phone,
            string email)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            City = city;
            State = state;
            PostalCode = postalCode;
            Phone = phone.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
            Email = ValidateEmail(email) ? email : throw new ArgumentException("Email address is Invalid", email);
        }

        // DDD Patterns comment
        // Using private fields to encapsulate and carefully manage data.
        // The only way to create an Buyer is through the constructor enabling
        // the domain class to enforce business rules and validation
        public int Id { get; private set; }
        public int OrderId { get; private set; }
        public string UserName { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string PostalCode { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }
        [JsonIgnore]
        public Order Order { get; private set; }
        [JsonIgnore]
        public PaymentMethod PaymentMethod { get; set; }

        // DDD Patterns comment:
        // This Buyer AggregateRoot's method "AddPaymentMethod()" should be the only way to payment information to the Buyer aggregate. This centralized approach provides consistency to the entire aggregate.
        public void AddPaymentMethod(string cardNumber, string securityCode, string cardHolderName, DateTime expirationDate)
        {
            PaymentMethod = new PaymentMethod(cardNumber, securityCode, cardHolderName, expirationDate);
        }


        // DDD Patterns comment
        // Any behavior(discounts, etc.) and validations are controlled by the Aggregate Root
        // in order to maintain consistency across the whole Aggregate.
        // This Order AggregateRoot's method "GetLineItemCount" is the only way to obtain a count
        // of items for an Order.;   

        // This Buyer AggregateRoot's method "ChangeShippingAddress()" should be the only way to change 
        // shipping information for an order
        public bool ChangeShippingAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                return false;
            }

            Address = address;
            return true;
        }

        // This Buyer AggregateRoot's method "ChangeShippingCity()" should be the only way to change the 
        // shipping information for an order
        public bool ChangeShippingCity(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return false;
            }

            Address = city;
            return true;
        }

        // This Buyer AggregateRoot's method "ChangeShippingState()" should be the only way to change the 
        // shipping information for an order
        public bool ChangeShippingState(string state)
        {
            if (string.IsNullOrEmpty(state))
            {
                return false;
            }

            Address = state;
            return true;
        }

        // This Buyer AggregateRoot's method "ChangeShippingZip()" should be the only way to change the 
        // shipping information for an order
        public bool ChangeShippingZip(string zip)
        {
            if (string.IsNullOrEmpty(zip))
            {
                return false;
            }

            Address = zip;
            return true;
        }

        // This Buyer AggregateRoot's method "ChangeShippingEmailAddress()" should be the only way to change the 
        // shipping information for an order
        public bool ChangeShippingEmailAddress(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
            {
                return false;
            }

            Address = ValidateEmail(emailAddress) ? emailAddress : throw new ArgumentException("Email address is Invalid", emailAddress); 
            return true;
        }

        private bool ValidateEmail(string emailAddress)
        {
            return (Regex.IsMatch(emailAddress,
                @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                RegexOptions.IgnoreCase));
        }
    }
}