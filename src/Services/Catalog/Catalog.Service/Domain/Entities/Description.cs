using System;
using System.Collections.Generic;

namespace Catalog.API.Domain.Entities
{
    public class Description
    {
        public Description()
        {
            Albums = new HashSet<Product>();
        }
        public int DescriptionId { get; set; }
        public string Item { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Product> Albums { get; set; }
    }
}