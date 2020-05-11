using Comment.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comment.API.Dto
{
    public class CommentDTO
    {
        public CommentDTO(PostComment comment, List<CommentDTO> replies)
        {
            Id = comment.Id;
            PostedDate = comment.PostedDate;
            Content = comment.Content;
            Author = comment.Author;
            Parents = comment.Parents;
            PostId = comment.PostId;
            Replies = replies;
        }
        public string Id { get; set; } 

        public DateTime PostedDate { get; set; }

        public string Content { get; set; }

        public Author Author { get; set; }

        public List<string> Parents { get; set; }
        public int PostId { get; set; }

        public List<CommentDTO> Replies { get; set; }
    }
}
