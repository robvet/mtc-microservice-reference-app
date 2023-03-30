using System;

namespace Basket.API.Dtos
{
    public class CheckoutDto
    {
        // Scalar properties (map to database columns)
        //[Required]
        public string BasketId { get; set; }
        public int OrderId { get; set; }
        public string PromoCode { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        //public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CreditCardNumber { get; set; }
        public string SecurityCode { get; set; }
        public string CardholderName { get; set; }
        public DateTime ExpirationDate { get; set; }
        
    }
}