﻿using System;

namespace Basket.Service.Domain.Entities
{
    public class BasketItemRemovedEntity
    {
        public Guid DeleteId { get; set; }
        public string Message { get; set; }
        public decimal BasketTotal { get; set; }
        public int ItemCount { get; set; }
    }
}