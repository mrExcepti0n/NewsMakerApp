using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comment.API.Models
{
    public class PostComment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonDateTimeOptions]
        public DateTime PostedDate { get; set; }

        public string Content { get; set; }

        public Author Author { get; set; }

        public List<string> Parents { get; set; } = new List<string>();

        public int PostId { get; set; }
    }
}
