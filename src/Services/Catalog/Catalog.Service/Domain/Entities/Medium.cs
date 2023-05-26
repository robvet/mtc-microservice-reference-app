using System;
using System.Collections.Generic;

namespace Catalog.API.Domain.Entities
{
    public class Medium
    {
        public Medium()
        {
            Products = new HashSet<Product>();
        }

        public int MediumId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}