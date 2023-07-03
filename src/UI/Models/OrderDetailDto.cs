using System;
using System.ComponentModel.DataAnnotations;

namespace MusicStore.Models
{
    public class OrderDetailDto
    {
        public string OrderId { get; set; }
       
        public Guid ProductId { get; set; }

        public Guid ArtistId { get; set; }

        [Display(Name="Artist")]
        public string Artist { get; set; }

        public Guid GenreId { get; set; }

        [Display(Name = "Genre")]
        public string Genre { get; set; }
        
        [Display(Name="Collection")]
        public string Title { get; set; }

        public int Quantity { get; set; }
        
        [Display(Name ="Price")]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

        public Guid MediumId { get; set; }

        [Display(Name = "Medium")]
        public string Medium { get; set; }

        [Display(Name = "Condition")]
        public string Condition { get; set; }
    }
}