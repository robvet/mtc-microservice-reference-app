using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Catalog.API.Domain.Entities
{
    public class ProductReadModel
    {
        public string Artist { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public string Medium { get; set; }
    }
}