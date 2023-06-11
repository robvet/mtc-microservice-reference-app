using System;
using System.Collections.Generic;

namespace catalog.service.Domain.Entities
{
    public class Description
    {

        public int DescriptionId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }

        public Product Product { get; set; }
    }
}