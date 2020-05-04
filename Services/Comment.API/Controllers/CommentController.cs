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
        public async Task<IEnumerable<PostComment>> Get(int postId)
        {
            return await _commentRepository.GetCommentsAsync(postId)?? new List<PostComment>();
        }

        [HttpPost]
        public async Task Post(AddCommentDTO postCommentDto)
        {
            var postComment = await _commentRepository.AddCommentAsync(postCommentDto);
            await _mediator.Send(new AddCommentCommand(postComment));
        }

    }
}
