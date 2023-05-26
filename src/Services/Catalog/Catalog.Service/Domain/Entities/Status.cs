using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Catalog.API.Domain.Entities
{
    public class Status
    {
        public Status()
        {
            Products = new HashSet<Product>();
        }
        public int StatusId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}