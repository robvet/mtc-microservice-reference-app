﻿namespace order.domain.Models;

using Newtonsoft.Json;

public abstract class Item
{
    //[JsonProperty("OrderId")]
    //public string OrderId { get; set; }
    
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }
}

