using Domain.Core.Model;
using Infrastructure.EventBus;
using NewsMaker.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsMaker.Web.IntegrationEvents
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
