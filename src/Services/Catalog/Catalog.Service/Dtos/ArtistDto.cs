using System.Runtime.Serialization;
using System;

namespace catalog.service.Dtos
{
    public class ArtistDto
    {
        public int ArtistId { get; set; }
        public Guid GuidId { get; set; }
        public string Name { get; set; }
    }
}