using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Data;
using NewsMaker.Web.Models;

namespace NewsMaker.Web.Controllers
{
    [Route("api/[controller]")]
    public class DictionaryController : Controller
    {
        private NewsContext _context;

        public DictionaryController(NewsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<DictionaryItem> GetShortList(TypeDictionaryEnum refType)
        {
            if (refType == TypeDictionaryEnum.Category)
                return _context.Categories.Select(c => new DictionaryItem(c.Id, c.Title)).ToList();

            throw new NotImplementedException();
        }
    }
}
