using AutoMapper;
using Domain.Core.Model;
using Infrastructure.EventBus.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NewsMaker.Web.IntegrationEvents;
using NewsMaker.Web.Models;
using Infrastructure.Data.Infrastructure;

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
        public async Task<NewsDto[]> Get()
        {
            var news = await _context.News.ToListAsync();
            return _mapper.Map<NewsDto[]>(news);
        }



        [HttpGet("[action]")]
        public async Task<int> Count()
        {
            return await _context.News.CountAsync();
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
