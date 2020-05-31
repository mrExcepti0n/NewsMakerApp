using Infrastructure.EventBus;
using NewsMaker.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;

namespace NewsMaker.Web.IntegrationEvents
{
    public class NewsUpdateEventHandler : IIntegrationEventHandler<NewsUpdateEvent>
    {
        private readonly SearchEngine _searchEngine;
        private readonly NewsContext _context;

        public NewsUpdateEventHandler(SearchEngine searchEngine, NewsContext context)
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
