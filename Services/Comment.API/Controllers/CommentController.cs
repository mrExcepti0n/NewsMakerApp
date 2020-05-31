using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly ICommentRepository _commentRepository;
        private readonly IMediator _mediator;

        public CommentController(ILogger<CommentController> logger, ICommentRepository commentRepository, IMediator mediator)
        {
            _logger = logger;
            _commentRepository = commentRepository;
            _mediator = mediator;
        }

        /// <summary>
        /// Get all comments with nested replies by postId
        /// </summary>
        /// <param name="postId"></param>   
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CommentDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetComments(int postId)
        {
            var postComments = (await _commentRepository.GetCommentsAsync(postId)).ToList();

            var comments = postComments.Where(pc => !pc.Parents.Any())
                                      .Select(pc => GetPostComment(pc, postComments));

            return Ok(comments.ToList());
        }

        /// <summary>
        /// Get comment by id without replies
        /// </summary>
        /// <param name="id"></param>  
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CommentDTO), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CommentDTO>> Get(string id)
        {
            var postComment = (await _commentRepository.GetComment(id));
            return Ok(new CommentDTO(postComment, new List<CommentDTO>()));
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

        /// <summary>
        /// Add comment
        /// </summary>
        /// <param name="postCommentDto"></param>  
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<CommentDTO>> Post(AddCommentDTO postCommentDto)
        {
            var postComment = await _commentRepository.AddCommentAsync(postCommentDto);
            var result = new CommentDTO(postComment, new List<CommentDTO>());
            await _mediator.Send(new AddCommentCommand(result));

            return CreatedAtAction(nameof(Get), new {id = postComment.Id}, postComment);
        }

    }
}
