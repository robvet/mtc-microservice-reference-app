﻿using System;

namespace Basket.Service.Domain.Entities
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Condition { get; set; }
        public string Medium { get; set; }
        public DateTime DateCreated { get; set; } = new DateTime();
    }
}