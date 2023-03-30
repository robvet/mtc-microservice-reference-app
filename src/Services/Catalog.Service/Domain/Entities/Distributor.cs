using System.Collections.Generic;

namespace Catalog.API.Domain.Entities
{
    public class Distributor
    {
        public Distributor()
        {
            Albums = new HashSet<Product>();
        }

        public int DistributorId { get; set; }
        public string Name { get; set; }

        public ICollection<Product> Albums { get; set; }
    }
}