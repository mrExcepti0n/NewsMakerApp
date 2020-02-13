using Data.Core.Model;
using EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.IntegrationEvents
{
    public class NewsAddEvent : IntegrationEvent
    {
        public NewsAddEvent(News news)
        {
            News = news;
        }

        public News News { get; set; }
    }
}
