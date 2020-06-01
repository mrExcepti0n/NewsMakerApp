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
using System.Net;
using System.Threading.Tasks;
using Infrastructure.Data;

namespace NewsMaker.Web.Controllers
{
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {

        private readonly NewsContext _context;
        private readonly IMapper _mapper;
        private readonly EventBusRabbitMQ _eventBus;

        public NewsController(NewsContext context, IMapper mapper, EventBusRabbitMQ eventBusRabbitMQ)
        {
            _context = context;
            _mapper = mapper;

            _eventBus = eventBusRabbitMQ;
        }

        /// <summary>
        /// Get news collection
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<NewsDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<NewsDto>>> Get(int? categoryId, int? skip, int? take)
        {
            var newsQuery = GetNewsQuery(categoryId, skip, take);

            var news = await newsQuery.ToListAsync();
            return _mapper.Map<List<NewsDto>>(news);
        }

        private IQueryable<News> GetNewsQuery(int? categoryId, int? skip, int? take)
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

            return newsQuery;
        }


        /// <summary>
        /// get news count
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> Count(int? categoryId)
        {
            return await _context.News.CountAsync(n => categoryId == null || n.CategoryId == categoryId);
        }


        /// <summary>
        /// Get news by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NewsDto), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<NewsDto>> Get(int id)
        {
            var news = await _context.News.SingleOrDefaultAsync(n => n.Id == id);
            return _mapper.Map<NewsDto>(news);
        }

        /// <summary>
        /// add new news
        /// </summary>
        /// <param name="newsDto"></param>
        /// <returns>newsId</returns>
        [HttpPost]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<int>> Add([FromBody]NewsDto newsDto)
        {
            var news = _mapper.Map<News>(newsDto);
            _context.Add(news);

            await _context.SaveChangesAsync();
            _eventBus.Publish(new NewsAddEvent(news));

            return CreatedAtAction(nameof(Get), new {id = news.Id}, news.Id);
        }

        /// <summary>
        /// delete news
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var news = await _context.Set<News>().FirstOrDefaultAsync(n => n.Id == id);

            if (news == null)
            {
                return NotFound();
            }

            _context.Remove(news);
            await _context.SaveChangesAsync();
            _eventBus.Publish(new NewsRemoveEvent(id));

            return NoContent();
        }

        /// <summary>
        /// update news
        /// </summary>
        /// <param name="newsDto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task Update([FromBody] NewsDto newsDto)
        {
            var news = _mapper.Map<News>(newsDto);
            _context.Update<News>(news);
            await _context.SaveChangesAsync();

            _eventBus.Publish(new NewsUpdateEvent(news));
        }
    }
}
