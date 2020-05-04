using AutoMapper;
using Domain.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsMaker.Web.Models;

namespace NewsMaker.Web.Configuration.Mapper
{
    public class NewsServiceProfiler : Profile
    {
        public NewsServiceProfiler()
        {
            CreateMap<News, NewsDto>();
            CreateMap<NewsDto, News>();
        }
    }
}
