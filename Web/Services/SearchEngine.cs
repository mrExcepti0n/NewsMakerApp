using Data.Core.Model;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public class SearchEngine
    {
        ElasticClient _client;

        public SearchEngine()
        {
            var settings = new ConnectionSettings().DefaultIndex("defaultindex");
            _client = new ElasticClient(settings);
            //List<News> dataCollection = DataService.GetNews();
            
           // var result = _client.IndexMany<News>(dataCollection);          
        }


        public IReadOnlyCollection<News> Search(string pattern)
        {
            var searchResponse = _client.Search<News>(s => s.From(0).Size(10).Query(q => q.Match(m => m.Field(f => f.Content).Query(pattern))));

            return searchResponse.Documents;
        }
    }
}
