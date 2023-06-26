using System;
using Newtonsoft.Json;

namespace EventBus.Events
{
    public class MessageEvent
    {
        public MessageEvent()
        {
            //Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        //[JsonConstructor]
        //public MessageEvent(Guid id, DateTime createDate)
        //{
        //    //Id = id;
        //    CreationDate = createDate;
        //}

        //[JsonProperty]
        //public Guid Id { get; private set; }

        [JsonProperty]
        public DateTime CreationDate { get; private set; }

        [JsonProperty]
        public string CorrelationToken { get; set; }

        [JsonProperty]
        public string MessageId { get; set; }
    }
}
