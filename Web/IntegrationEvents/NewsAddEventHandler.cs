using EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Services;

namespace Web.IntegrationEvents
{
    public class NewsAddEventHandler : IIntegrationEventHandler<NewsAddEvent>
    {
        public SearchEngine _searchEngine;

        public NewsAddEventHandler(SearchEngine searchEngine)
        {
            _searchEngine = searchEngine;
        }


        public async Task Handle(NewsAddEvent @event)
        {
           await _searchEngine.Add(@event.News);
        }
    }
}
