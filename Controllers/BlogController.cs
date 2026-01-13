using Active_Blog_Service_API.Dto;
using Active_Blog_Service_API.Helpers;
using Active_Blog_Service_API.Repositories.Contract;
using Active_Blog_Service_API.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Active_Blog_Service_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        [HttpGet("blog/index")]
        public async Task<IActionResult> Index()
        {
            var blogs = await _blogService.GetAllBlogsServiceAsync();

            return Ok(blogs);
        }
        [HttpPost("blog/add")]
        public async Task<IActionResult> AddBlog([FromBody] AddBlogDto addBlogDto)
        {
            if (ModelState.IsValid)
            {
                await _blogService.AddBlogServiceAsync(User, Request, addBlogDto);
                return Ok("Add Blog Successfully");
            }
            return BadRequest("Data is wrong try again later!");

        }
        [HttpPut("blog/edit")]
        public async Task<IActionResult> EditBlog([FromQuery] int id, [FromBody] EditBlogDto editBlogDto)
        {
            if (ModelState.IsValid)
            {


                var result = await _blogService.UpdateBlogServiceAsync(User, id, editBlogDto);
                if (result.Succeeded)
                    return Ok("Blog Edited Successfully!");
                else
                    return BadRequest(result.Errors);
            }
            return BadRequest("Data is wrong try again later!");

        }
        [HttpGet("blog/details")]
        public async Task<IActionResult> BlogDetail([FromQuery] int id)
        {
            var blogDetails = await _blogService.ShowBlogServiceAsync(User, Request, id);

            return Ok(blogDetails);
        }
        [HttpPost("blog/delete")]
        public async Task<IActionResult> DeleteBlog([FromQuery] int id)
        {

            var result = await _blogService.DeleteServiceAsync(User, id);
            if (result.Succeeded)
                return Ok("Blog deleted successfuly!");
            return BadRequest(result.Errors); ;

        }


    }
}
