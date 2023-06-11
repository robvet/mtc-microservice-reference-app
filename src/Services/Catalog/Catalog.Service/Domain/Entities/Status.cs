using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace catalog.service.Domain.Entities
{
    [Index(nameof(Name), IsUnique = true)]
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