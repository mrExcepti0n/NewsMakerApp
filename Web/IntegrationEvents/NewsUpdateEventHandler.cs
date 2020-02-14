using Data.Core.Infrastructure;
using EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Services;

namespace Web.IntegrationEvents
{
    public class NewsUpdateEventHandler : IIntegrationEventHandler<NewsUpdateEvent>
    {
        private SearchEngine _searchEngine;
        private DataContext _context;

        public NewsUpdateEventHandler(SearchEngine searchEngine, DataContext context)
        {
            _searchEngine = searchEngine;
            _context = context;
        }


        public async Task Handle(NewsUpdateEvent @event)
        {

            var news = @event.News;

            if (news.Category == null)
            {
                news.Category = _context.Categories.SingleOrDefault(c => c.Id == news.CategoryId);
            }

            await _searchEngine.Update(@event.News);
        }
    }
}
