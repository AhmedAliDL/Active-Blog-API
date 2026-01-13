using Active_Blog_Service.Models;
using Active_Blog_Service_API.Dto;
using Active_Blog_Service_API.Helpers;
using Active_Blog_Service_API.Repositories.Contract;
using Active_Blog_Service_API.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Active_Blog_Service_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {

        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpGet("comment/index")]
        public async Task<IActionResult> Index([FromQuery] int blogId)
        {
            var comments = await _commentService.GetCommentsBelongsToBlog(blogId);
            return Ok(comments);
        }
        [HttpPost("comment/addComment")]
        public async Task<IActionResult> AddComment([FromBody] AddCommentDto addCommentDto)
        {
            if (ModelState.IsValid)
            {

                await _commentService.AddCommentServiceAsync(User, Request, addCommentDto);
                return Ok("Comment successfuly added!");
            }
            return BadRequest("Data is wrong try again!");
        }
        [HttpPut("comment/editComment")]
        public async Task<IActionResult> EditComment([FromBody] EditCommentDto editCommentDto)
        {
            if (ModelState.IsValid)
            {
                var result =  await _commentService.UpdateCommentServiceAsync(User, editCommentDto);
                if(result.Succeeded)
                     return Ok("Comment successfuly edited!");
                else
                    return BadRequest(result.Errors);
            }
            return BadRequest("Data is wrong try again!");
        }
        [HttpPost("comment/deleteComment")]
        public async Task<IActionResult> DeleteComment([FromQuery] int blogId, [FromQuery] int commentId)
        {
            if (ModelState.IsValid)
            {
                var result = await _commentService.DeleteCommentServiceAsync(User, blogId, commentId);
                if (result.Succeeded)
                    return Ok("Comment successfuly deleted!");
                else
                    return BadRequest(result.Errors); ;
            }
            return BadRequest("Data is wrong try again!");
            
        }
    }
}
