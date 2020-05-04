using Domain.Core.Model;

namespace NewsMaker.Web.Models
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
