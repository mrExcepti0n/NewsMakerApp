
using Comment.API.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Comment.API.Models;

namespace Comment.API.Infrastructure
{
    public class CommentContext
    {
        private readonly IMongoDatabase _database = null;

        public CommentContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);

            var indexModel = new CreateIndexModel<PostComment>(Builders<PostComment>.IndexKeys.Ascending(c => c.PostId));
            PostComments.Indexes.CreateOne(indexModel);
        }

        public IMongoCollection<PostComment> PostComments
        {
            get
            {
                return _database.GetCollection<PostComment>("PostComment");
            }
        }
    }
}
