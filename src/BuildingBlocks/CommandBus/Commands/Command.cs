using System;
using Newtonsoft.Json;

namespace CommandBus.Commands
{
    public class Command
    {
        public Command()
        {
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public Command(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public DateTime CreationDate { get; private set; }

        [JsonProperty]
        public string CorrelationToken { get; set; }

        //[JsonProperty]
        //public string Event { get; set; }
    }
}
