using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MusicStore.Models
{
    public class AlbumDTO
    {

        public int Id { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public string AlbumArtUrl { get; set; }

        public bool ParentalCaution { get; set; }

        [Required]
        [StringLength(10)]
        public string Upc { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public decimal Price { get; set; }

        public int Available { get; set; }

        [Required]
        public bool Cutout { get; set; }

        [StringLength(100)]
        public string ArtistName { get; set; }

        public int ArtistId { get; set; }

        public string GenreName { get; set; }

        public int GenreId { get; set; }

    }
}