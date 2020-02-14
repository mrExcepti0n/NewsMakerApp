using Data.Core.Model;
using EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Services;

namespace Web.IntegrationEvents
{
    public class NewsRemoveEventHandler : IIntegrationEventHandler<NewsRemoveEvent>
    {
        private SearchEngine _searchEngine;

        public NewsRemoveEventHandler(SearchEngine searchEngine)
        {
            _searchEngine = searchEngine;
        }


        public async Task Handle(NewsRemoveEvent @event)
        {
            await _searchEngine.Remove<News>(@event.NewsId);
        }
    }
}
