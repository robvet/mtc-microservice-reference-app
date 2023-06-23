﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace catalog.service.Domain.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    [Index(nameof(GuidId), IsUnique = true)]
    public class Condition
    {
        public Condition()
        {
            Products = new HashSet<Product>();
        }

        public int ConditionId { get; set; }
        public Guid GuidId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}