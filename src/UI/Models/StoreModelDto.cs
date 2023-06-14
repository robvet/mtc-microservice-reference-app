using System.Collections.Generic;

namespace MusicStore.Models
{
    public class StoreModelDto
    {
        public string GenreName { get; set; }
        public List<ProductDto> Result { get; set; }
    }
}
