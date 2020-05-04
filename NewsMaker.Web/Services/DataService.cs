using Domain.Core.Model;
using System.Collections.Generic;

namespace NewsMaker.Web.Services
{
    public static class DataService
    {
        public static List<News> GetNews() => new List<News>
        {
            new News
            {
                Id = 1,
                Category = new Category {Title = "A" },
                Header = "Самые большие",
                Content = "Самые большие"
            },
            new News
            {
                Id = 2,
                Category = new Category {Title ="A" },
                Header = "Самые маленькие",
                Content = "Самые маленькие"
            },
            new News
            {
                Id = 3,
                Category = new Category {Title ="B" },
                Header = "Впервые",
                Content = "«Первый канал» впервые представит сериал в свободном доступ"
            },
            new News
            {
                Id = 4,
                Category = new Category { Title = "C" },
                Header = "Здоровье",
                Content = "Съеште корень солодки"
            },
        };
    }
}
