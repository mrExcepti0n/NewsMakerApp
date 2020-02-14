using Data.Core.Model;
using EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.IntegrationEvents
{
    public class NewsUpdateEvent : IntegrationEvent
    {
        public NewsUpdateEvent(News news)
        {
            News = news;
        }

        public News News { get; set; }
    }
}
