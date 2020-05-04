﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using NewsMaker.Web.Models;
using Infrastructure.Data.Infrastructure;

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
        public List<KeyValuePair<int, string>> GetShortList(TypeDictionaryEnum refType)
        {
            if (refType == TypeDictionaryEnum.Category)
                return _context.Categories.Select(c => new KeyValuePair<int, string>(c.Id, c.Title)).ToList();

            throw new NotImplementedException();
        }
    }
}