﻿using System;
using EventBus.Events;

namespace catalog.service.Events
{
    public class ProductChangedEvent : MessageEvent
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool ParentalCaution { get; set; }
        public string Upc { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public bool? Cutout { get; set; }
        public string ArtistName { get; set; }
        public string GenreName { get; set; }
    }
}