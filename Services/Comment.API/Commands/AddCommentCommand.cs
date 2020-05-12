using Comment.API.Dto;
using Comment.API.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comment.API.Commands
{
    public class AddCommentCommand : IRequest<bool>
    {
        public CommentDTO Comment { get; private set; }

        public AddCommentCommand() 
        { 
        }

        public AddCommentCommand(CommentDTO postComment)
        {
            Comment = postComment;
        }
    }
}
