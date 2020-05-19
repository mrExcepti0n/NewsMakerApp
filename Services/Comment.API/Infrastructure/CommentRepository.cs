using Comment.API.Dto;
using Comment.API.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comment.API.Infrastructure
{
    public class CommentRepository : ICommentRepository
    {
        private readonly CommentContext _context;

        public CommentRepository(CommentContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PostComment>> GetCommentsAsync(int postId)
        {
            var query = _context.PostComments.Find(comment => comment.PostId == postId);
            return await query.ToListAsync();
        }

        public async Task<PostComment> GetCommentAsync(string id)
        {
            var query = _context.PostComments.Find(comment => comment.Id == id);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<PostComment> AddCommentAsync(AddCommentDTO item)
        {
            List<string> parents = new List<string>();
            if (!string.IsNullOrWhiteSpace(item.ParentId)) {
                var parentComment = await GetCommentAsync(item.ParentId);

                parents = parentComment.Parents.ToList();
                parents.Add(parentComment.Id);
            }

            var postComment = new PostComment { PostedDate = item.PostedDate, Content = item.Content, PostId = item.PostId, Author = item.Author, Parents = parents };



            await _context.PostComments.InsertOneAsync(postComment);
            return postComment;
        }
    }
}
