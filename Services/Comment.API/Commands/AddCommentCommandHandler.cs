using Comment.API.SignalR;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Comment.API.Commands
{
    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, bool>
    {
        private readonly IHubContext<PostCommentingHub> _hubContext;

        public AddCommentCommandHandler(IHubContext<PostCommentingHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task<bool> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            string group = $"post{request.Comment.PostId}";
            await _hubContext.Clients.Group(group)
              .SendAsync("PostComment", request.Comment);
            return true;
        }
    }
}
