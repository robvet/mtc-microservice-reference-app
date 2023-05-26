using System;
using System.Collections.Generic;

namespace Catalog.API.Domain.Entities
{
    public class Condition
    {
        public Condition()
        {
            Products = new HashSet<Product>();
        }

        public int ConditionId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}