﻿using System;
using System.Collections.Generic;

namespace Catalog.API.Domain.Entities
{
    public class Genre
    {
        public Genre()
        {
            Albums = new HashSet<Product>();
        }

        public int GenreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Product> Albums { get; set; }
    }
}