using Comment.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comment.API.Dto
{
    public class AddCommentDTO
    {
        public DateTime PostedDate { get; set; }

        public string Content { get; set; }

        public Author Author { get; set; }

        public string ReplyId { get; set; }
        public int PostId { get; set; }
    }
}
