using System;
using System.Collections.Generic;

namespace MusicStore.Models
{
    public class ArtistDto
    {
        public int ArtistId { get; set; }
        public Guid GuidId { get; set; }
        public string Name { get; set; }
    }
}
