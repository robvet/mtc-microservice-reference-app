using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace catalog.service.Dtos
{
    [DataContract]
    public class GenreDto
    {
        [DataMember] public int GenreId { get; set; }

        [DataMember] public Guid GuidId { get; set; }

        [DataMember] public string Name { get; set; }

        [DataMember] public string Description { get; set; }

        [DataMember] public List<ProductDto> Albums { get; set; }
    }
}