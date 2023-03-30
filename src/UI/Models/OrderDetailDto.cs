using System.ComponentModel.DataAnnotations;

namespace MusicStore.Models
{
    public class OrderDetailDto
    {
        public int OrderDetailId { get; set; }
        public string OrderId { get; set; }
        public int AlbumId { get; set; }
        public string AlbumArtUrl { get; set; }
        [Display(Name="Artist")]
        public string Artist { get; set; }
        [Display(Name="Song")]
        public string Title { get; set; }

        public int Quantity { get; set; }
        [Display(Name ="Price")]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }
    }
}