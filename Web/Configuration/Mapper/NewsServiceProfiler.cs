using AutoMapper;
using Data.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Configuration.Mapper
{
    public class NewsServiceProfiler : Profile
    {
        public NewsServiceProfiler()
        {
            CreateMap<News, NewsDto>().ForMember(dst => dst.Category, opt => opt.MapFrom(src => src.Category.Title));
        }
    }
}
