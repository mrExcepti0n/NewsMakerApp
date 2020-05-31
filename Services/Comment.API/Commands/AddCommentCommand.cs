using Comment.API.Dto;
using MediatR;

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
