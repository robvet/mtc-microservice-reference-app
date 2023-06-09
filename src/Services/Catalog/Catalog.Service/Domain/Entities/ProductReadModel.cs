﻿using System;

namespace catalog.service.Domain.Entities
{
    public class ProductReadModel
    {
        public Guid ProductId { get; set; }
        public Guid ArtistId { get; set; }
        public string Artist { get; set; }
        public Guid GenreId { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public  string Status { get; set; }
        public string Condition { get; set; }
        public Guid MediumId { get; set; }
        public string Medium { get; set; }
        public bool HighValueItem { get; set; }
    }
}
