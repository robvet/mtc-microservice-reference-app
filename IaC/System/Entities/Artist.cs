﻿using System.Collections.Generic;

namespace Tools.Entities
{
    public class Artist
    {
        public Artist()
        {
            Albums = new HashSet<Product>();
        }

        public int ArtistId { get; set; }
        public string Name { get; set; }

        public ICollection<Product> Albums { get; set; }
    }
}