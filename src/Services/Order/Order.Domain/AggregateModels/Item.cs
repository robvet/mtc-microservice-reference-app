namespace order.domain.AggregateModels;

using Newtonsoft.Json;

public abstract class Item
{
    //[JsonProperty("id")]
    [JsonProperty("OrderId")]
    //public string Id { get; set; }
    public string OrderId { get; set; }
}

