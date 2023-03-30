using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace AdminTools.Entities
{
    /// <summary>
    ///     This class maps to an Azure Table Entity
    ///     "MusicProduct" => Partition Key
    ///     ProductId => Row Key
    /// </summary>
    public class ProductEntity : TableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool ParentalCaution { get; set; }
        public string Upc { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Price { get; set; }
        public bool? Cutout { get; set; }
        public string ArtistName { get; set; }
        public string GenreName { get; set; }
    }
}