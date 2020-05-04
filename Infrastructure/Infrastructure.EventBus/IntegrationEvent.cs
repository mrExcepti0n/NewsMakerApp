using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.EventBus
{
    public class IntegrationEvent
    {

        [JsonProperty]
        public Guid Id { get; private set; }
        [JsonProperty]

        public DateTime CreationDate { get; private set; }
    }
}
