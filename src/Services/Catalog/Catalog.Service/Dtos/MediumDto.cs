using System.Collections.Generic;
using System.Runtime.Serialization;
using System;

namespace catalog.service.Dtos
{
    public class MediumDto
    {
        [DataMember] public int MediumId { get; set; }

        [DataMember] public Guid GuidId { get; set; }

        [DataMember] public string Name { get; set; }

        [DataMember] public string Description { get; set; }
    }
}
