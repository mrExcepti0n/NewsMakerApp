using Domain.Core.Model;
using Infrastructure.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsMaker.Web.IntegrationEvents
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
