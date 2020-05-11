using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comment.API.Commands;
using Comment.API.Dto;
using Comment.API.Infrastructure;
using Comment.API.Models;
using Comment.API.SignalR;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Comment.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ILogger<CommentController> _logger;
        private ICommentRepository _commentRepository;
        private readonly IMediator _mediator;

        public CommentController(ILogger<CommentController> logger, ICommentRepository commentRepository, IMediator mediator)
        {
            _logger = logger;
            _commentRepository = commentRepository;
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IEnumerable<CommentDTO>> Get(int postId)
        {
            var postComments = await _commentRepository.GetCommentsAsync(postId)?? new List<PostComment>();

            var commets = postComments.Where(pc => !pc.Parents.Any())
                                      .Select(pc => GetPostComment(pc, postComments.ToList()));

            return commets.ToList();

        }

        private CommentDTO GetPostComment(PostComment comment, List<PostComment> allComments)
        {
            var replies = GetReplies(comment, allComments).ToList();
            return new CommentDTO(comment, replies);
        }

        private IEnumerable<CommentDTO> GetReplies(PostComment comment, List<PostComment> allComments, int lvl = 0)
        {
            foreach (var reply in allComments.Where(c => c.Parents.Count == lvl+1 && c.Parents[lvl] == comment.Id))
            {
                var replies = GetReplies(reply, allComments, lvl +1).ToList();
                yield return new CommentDTO(reply, replies);
            }
        }

        [HttpPost]
        public async Task<CommentDTO> Post(AddCommentDTO postCommentDto)
        {
            var postComment = await _commentRepository.AddCommentAsync(postCommentDto);
            await _mediator.Send(new AddCommentCommand(postComment));

            return new CommentDTO(postComment, new List<CommentDTO>());
        }

    }
}
