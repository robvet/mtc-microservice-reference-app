using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Catalog.API.Dtos
{
    [DataContract]
    public class GenreDto
    {
        [DataMember] public int GenreId { get; set; }

        [DataMember] public string Name { get; set; }

        [DataMember] public string Description { get; set; }

        [DataMember] public List<ProductDto> Albums { get; set; }
    }
}