using Domain.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NewsMaker.Web.Models
{
    public class SearchResult
    {
        public SearchResult(IEnumerable<News> news)
        {
            SearchGroupResults  = news.GroupBy(n => n.Category.Title).Select(gc => new SearchGroupResult(gc)).ToList();
        }
        public List<SearchGroupResult> SearchGroupResults { get; set; }
    }
}
