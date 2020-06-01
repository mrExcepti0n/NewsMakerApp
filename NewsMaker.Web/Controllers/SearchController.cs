using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Domain.Core.Model;
using NewsMaker.Web.Models;
using NewsMaker.Web.Services;

namespace NewsMaker.Web.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly SearchEngine _searchEngine;

        public SearchController(SearchEngine searchEngine)
        {
            _searchEngine = searchEngine;
        }


        /// <summary>
        /// Find news by content
        /// </summary>
        /// <param name="pattern">content</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(SearchResult), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<SearchResult>> Get(string pattern)
        {
            IReadOnlyCollection<News> requestedData = new List<News>();
            if (!string.IsNullOrWhiteSpace(pattern))
            {
                requestedData = await _searchEngine.SearchAsync(pattern);
            }
            return new SearchResult(requestedData);
        }
    }
}
