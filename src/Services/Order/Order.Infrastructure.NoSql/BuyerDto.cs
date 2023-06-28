using System;

namespace order.infrastructure.nosql
{
    public class BuyerDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public OrderDto OrderDto { get; set; }
    }
}