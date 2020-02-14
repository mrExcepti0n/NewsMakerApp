using EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.IntegrationEvents
{
    public class NewsRemoveEvent : IntegrationEvent
    {
        public NewsRemoveEvent(int newsId)
        {
            NewsId = newsId;
        }

        public int NewsId { get; set; }
    }
}
