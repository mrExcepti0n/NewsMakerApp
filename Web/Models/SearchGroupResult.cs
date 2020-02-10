using Data.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace Web.Models
{
    public class SearchGroupResult
    {

        public SearchGroupResult(IGrouping<string, News> groupedNews)
        {

            GroupTitle = groupedNews.Key;
            SearchContentResults = groupedNews.Select(gc => new SearchContentResult(gc)).ToList();
        }

        public string GroupTitle { get; set; }
        public List<SearchContentResult> SearchContentResults { get; set; }
    }
}
