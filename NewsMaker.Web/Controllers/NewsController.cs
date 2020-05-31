using AutoMapper;
using Domain.Core.Model;
using Infrastructure.EventBus.RabbitMQ;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsMaker.Web.IntegrationEvents;
using NewsMaker.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;

namespace NewsMaker.Web.Controllers
{
    [Route("api/[controller]")]
    public class NewsController : Controller
    {

        private NewsContext _context;
        private IMapper _mapper;
        private EventBusRabbitMQ _eventBus;

        public NewsController(NewsContext context, IMapper mapper, EventBusRabbitMQ eventBusRabbitMQ)
        {
            _context = context;
            _mapper = mapper;

            _eventBus = eventBusRabbitMQ;
        }


        [HttpGet]
        public async Task<NewsDto[]> Get(int? categoryId, int? skip, int? take)
        {
            var predicateBuilder = PredicateBuilder.New<News>(true);
            if (categoryId.HasValue)
            {
                predicateBuilder.And(n => n.CategoryId == categoryId);
            }

            var newsQuery = _context.News.Where(predicateBuilder);

            if (skip.HasValue)
            {
                newsQuery = newsQuery.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                newsQuery = newsQuery.Take(take.Value);
            }


            var news = await newsQuery.ToListAsync();
            return _mapper.Map<NewsDto[]>(news);
        }



        [HttpGet("[action]")]
        public async Task<int> Count(int? categoryId)
        {
            return await _context.News.CountAsync(n => categoryId == null || n.CategoryId == categoryId);
        }



        [HttpGet("{id}")]
        public async Task<NewsDto> Get(int id)
        {
            var news = await _context.News.SingleOrDefaultAsync(n => n.Id == id);
            return _mapper.Map<NewsDto>(news);
        }


        [HttpPost]
        public async Task<int> Add([FromBody]NewsDto newsDto)
        {
            var news = _mapper.Map<News>(newsDto);
            _context.Add(news);

            await _context.SaveChangesAsync();
            _eventBus.Publish(new NewsAddEvent(news));

            return news.Id;
        }


        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var news = await _context.Set<News>().FirstOrDefaultAsync(n => n.Id == id);

            if (news != null)
            {
                _context.Remove(news);
                await _context.SaveChangesAsync();
            }

            _eventBus.Publish(new NewsRemoveEvent(id));


        }

        [HttpPut]
        public async Task Update([FromBody] NewsDto newsDto)
        {
            var news = _mapper.Map<News>(newsDto);
            _context.Update<News>(news);
            await _context.SaveChangesAsync();

            _eventBus.Publish(new NewsUpdateEvent(news));
        }
    }
}
