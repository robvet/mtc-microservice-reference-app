using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace catalog.service.Domain.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Genre
    {
        public Genre()
        {
            Products = new HashSet<Product>();
        }

        public int GenreId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}