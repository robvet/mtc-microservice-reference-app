using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Models
{
    public class GenreDto
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<AlbumDTO> Albums { get; set; }
    }
}
