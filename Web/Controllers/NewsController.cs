using AutoMapper;
using Data.Core.Infrastructure;
using Data.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class NewsController : Controller 
    {

        private DataContext _context;
        private IMapper _mapper;
        public NewsController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<NewsDto[]> Get()
        {
            var news = await _context.News.Include(n => n.Category).ToListAsync();
            return _mapper.Map<NewsDto[]>(news);
        }

        [HttpPost]
        public async Task Add(News news)
        {
            _context.Add(news);
            await _context.SaveChangesAsync();
        }

        [HttpPut]
        public async Task Update([FromBody] NewsDto newsDto)
        {

            var news = _mapper.Map<News>(newsDto);
            _context.Update<News>(news);
            await _context.SaveChangesAsync();
        }
    }
}
