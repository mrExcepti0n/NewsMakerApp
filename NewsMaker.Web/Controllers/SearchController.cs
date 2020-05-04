using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Domain.Core.Model;
using NewsMaker.Web.Models;
using NewsMaker.Web.Services;

namespace NewsMaker.Web.Controllers
{
    [Route("api/[controller]")]
    public class SearchController
    {
        private SearchEngine _searchEngine;

        public SearchController(SearchEngine searchEngine)
        {
            _searchEngine = searchEngine;
        }


        
        [HttpGet]
        public SearchResult Searh(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
            {
                return new SearchResult(new List<News>());
            }
            var requestedData = _searchEngine.Search(pattern);
            return new SearchResult(requestedData);
        }
    }
}
