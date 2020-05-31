using Comment.API.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comment.API.SignalR
{
    public class PostCommentingHub : Hub
    {
        private string GetGroupName(int postId)
        {
            return $"post{postId}";
        }

        public Task PostComment(PostComment comment)
        {
            return Clients.Group(GetGroupName(comment.PostId)).SendAsync("Send", comment);
        }

        public async Task JoinGroup(int postId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(postId));
        }

        public async Task LeaveGroup(int postId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(postId));
        }
    }
}

