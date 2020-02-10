using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Data.Core.Model;
using Web.Models;
using Web.Services;

namespace Web.Controllers
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
