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
        public PostComment Comment { get; private set; }

        public AddCommentCommand() 
        { 
        }

        public AddCommentCommand(PostComment postComment)
        {
            Comment = postComment;
        }
    }
}
