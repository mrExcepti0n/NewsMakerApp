using Comment.API.Dto;
using Comment.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comment.API.Infrastructure
{
    public interface ICommentRepository
    {
        Task<IEnumerable<PostComment>> GetCommentsAsync(int postId);
        Task<PostComment> AddCommentAsync(AddCommentDTO item);
    }
}
