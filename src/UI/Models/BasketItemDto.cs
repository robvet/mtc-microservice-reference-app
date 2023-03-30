using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Models
{
    public class BasketItemDto
    {
        public string BasketId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int QuanityOrdered { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public bool ParentalCaution { get; set; }
        public string Etag { get; set; }
    }
}
