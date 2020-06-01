using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Infrastructure.Data;
using NewsMaker.Web.Models;

namespace NewsMaker.Web.Controllers
{
    [Route("api/[controller]")]
    public class DictionaryController : ControllerBase
    {
        private readonly NewsContext _context;

        public DictionaryController(NewsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get dictionary
        /// </summary>
        /// <param name="refType">dictionary type enum</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<DictionaryItem>), (int)HttpStatusCode.OK)]
        public ActionResult<List<DictionaryItem>> GetShortList(TypeDictionaryEnum refType)
        {
            if (refType == TypeDictionaryEnum.Category)
                return _context.Categories.Select(c => new DictionaryItem(c.Id, c.Title)).ToList();

            throw new NotImplementedException();
        }
    }
}
