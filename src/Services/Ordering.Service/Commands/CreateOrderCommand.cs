using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Ordering.API.Dtos;

namespace Ordering.API.Commands
{
    // DDD and CQRS patterns comment: Note that it is recommended to implement immutable Commands
    // In this case, its immutability is achieved by having all the setters as private
    // plus only being able to update the data just once, when creating the object through its constructor.
    // References on Immutable Commands:  
    // http://cqrs.nu/Faq
    // https://docs.spine3.org/motivation/immutability.html 
    // http://blog.gauffin.org/2012/06/griffin-container-introducing-command-support/
    // https://docs.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/how-to-implement-a-lightweight-class-with-auto-implemented-properties
    [DataContract]
    public class CreateOrderCommand
    {
        public CreateOrderCommand(
            string basketId,
            string checkoutId,
            string userName,
            decimal total,
            string firstName,
            string lastName,
            string address,
            string city,
            string state,
            string postalCode,
            string phone,
            string email,
            string creditCardNumber,
            string securityCode,
            string cardholderName,
            DateTime expirationDate,
            string correlationToken,
            List<OrderDetailDto> orderDetails)
        {
            BasketId = basketId;
            CheckoutId = checkoutId;
            Total = total;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            City = city;
            State = state;
            PostalCode = postalCode;
            Phone = phone;
            Email = email;
            CreditCardNumber = creditCardNumber;
            SecurityCode = securityCode;
            CardholderName = cardholderName;
            ExpirationDate = expirationDate;
            CorrelationToken = correlationToken;
            OrderDetails = orderDetails;
        }

        [DataMember] public string BasketId { get; }

        [DataMember] public string CheckoutId { get; }

        [DataMember] public decimal Total { get; }

        [DataMember] public string CorrelationToken { get; }

        [DataMember] public List<OrderDetailDto> OrderDetails { get; }

        [DataMember] public string UserName { get; }

        [DataMember] public string FirstName { get; }

        [DataMember] public string LastName { get; }

        [DataMember] public string Address { get; }

        [DataMember] public string City { get; }

        [DataMember] public string State { get; }

        [DataMember] public string PostalCode { get; }

        [DataMember] public string Phone { get; }

        [DataMember] public string Email { get; }

        public string CreditCardNumber { get; }
        public string SecurityCode { get; }
        public string CardholderName { get; }
        public DateTime ExpirationDate { get; }
    }
}