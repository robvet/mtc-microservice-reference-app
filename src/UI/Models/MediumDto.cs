using System;
using System.Collections.Generic;

namespace MusicStore.Models
{
    public class GenreDto
    {
        public int GenreId { get; set; }
        public Guid GuidId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
