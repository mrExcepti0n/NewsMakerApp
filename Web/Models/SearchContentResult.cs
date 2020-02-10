using Data.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class SearchContentResult
    {

        public SearchContentResult(News news)
        {
            Header = news.Header;
            Content = news.Content;
        }
        public string Header { get; set; }

        public string Content { get; set; }
    }
}
