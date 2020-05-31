using Domain.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class NewsContext : DbContext
    {
        public NewsContext(DbContextOptions<NewsContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<News> News { get; set; }
    }
}
