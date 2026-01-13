using Active_Blog_Service.Exceptions;
using Active_Blog_Service.Models;
using Active_Blog_Service_API.Dto;
using Active_Blog_Service_API.Helpers;
using Active_Blog_Service_API.Repositories.Contract;
using Active_Blog_Service_API.Services.Contracts;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Active_Blog_Service_API.Services
{

    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly UserManager<User> _userManager;

        public BlogService(IBlogRepository blogRepository, ICommentRepository commentRepository,
            UserManager<User> userManager)
        {
            _blogRepository = blogRepository;
            _commentRepository = commentRepository;
            _userManager = userManager;
        }
        public async Task<List<Blog>> GetAllBlogsServiceAsync()
        {
            return await _blogRepository.GetAllBlogsAsync();
        }
        public async Task<Blog> GetBlogServiceAsync(int id)
        {
            return await _blogRepository.GetBlogByIdAsync(id);
        }
        public async Task AddBlogServiceAsync(ClaimsPrincipal User, HttpRequest request
            , AddBlogDto addBlogDto)
        {
            Blog blog = new()
            {
                Title = addBlogDto.Title,
                Category = addBlogDto.Category,
                BlogContent = addBlogDto.BlogContent
            };


            var user = await _userManager.GetUserAsync(User);
            blog.UserId = user!.Id;
            if (addBlogDto.Image != null && addBlogDto.Image.Length > 0)
            {
                try
                {

                    var uploadsfolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/BlogImages");
                    var uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(addBlogDto.Image.FileName);
                    var filePath = Path.Combine(uploadsfolder, uniqueFileName);
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await addBlogDto.Image.CopyToAsync(stream);
                    }

                    blog.Image = $"/BlogImages/{uniqueFileName}";
                }

                catch (IOException ex)
                {
                    throw new ImageUploadException("Image upload failed. Please try again.", ex);
                }
            }
            else
                blog.Image = $"/BlogImages/Default.png";
            await _blogRepository.AddBlogAsync(blog);

        }
        public async Task<IdentityResult> UpdateBlogServiceAsync(ClaimsPrincipal User, int id, EditBlogDto newBlog)
        {
            User? user = await _userManager.GetUserAsync(User);
            Blog oldBlog = await _blogRepository.GetBlogByIdAsync(id);
            if(oldBlog == null)
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "BlogNotFound",
                    Description = "Blog Not Found."
                }
                   );
            bool checkUserPermission = oldBlog.UserId == user!.Id;
            if (checkUserPermission)
            {
                oldBlog.Title = newBlog.Title ?? oldBlog.Title;
                oldBlog.BlogContent = newBlog.BlogContent ?? oldBlog.BlogContent;
                oldBlog.Category = newBlog.Category ?? oldBlog.Category;
                if (newBlog.Image is not null)
                {
                    try
                    {


                        var uploadsfolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/BlogImages");
                        var uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(newBlog.Image.FileName);
                        var filePath = Path.Combine(uploadsfolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await newBlog.Image.CopyToAsync(stream);
                        }
                        oldBlog.Image = $"/BlogImages/{uniqueFileName}";
                    }
                    catch (IOException ex)
                    {
                        throw new ImageUploadException("Failed to upload image.Please try again.", ex);
                    }
                }
                await _blogRepository.UpdateBlogAsync(oldBlog);
                return IdentityResult.Success;
            }
            else
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "BlogUpdateNotAllowed",
                    Description = "You Don`t have permission to edit this blog."
                }
                    );

        }
        public async Task<BlogDetailsDto> ShowBlogServiceAsync(ClaimsPrincipal User, HttpRequest request, int id)
        {
            Blog? blog = await _blogRepository
                .GetBlogByIdAsync(id);
            var blogDetails = new BlogDetailsDto();
            if (blog != null)
            {
                blogDetails.Title = blog.Title;
                blogDetails.CreatedDate = blog.CreatedDate;
                blogDetails.Category = blog.Category;
                blogDetails.BlogImage = blog.Image!;
                blogDetails.BlogContent = blog.BlogContent;
                var commentsDto = await _commentRepository
                    .GetCommentsOfBlogOrderByDateTime(id)
                    .Select(c => new CommentDetailsDto
                    {
                        UserName = $"{c.User.FName} {c.User.LName}",
                        CommentContent = c.CommentContent,
                        UserImage = c.User.Image,
                        CommentDate = c.CreatedDateTime
                    }
                    ).ToListAsync();
                blogDetails.BlogComments = commentsDto;
                blogDetails.UserImage = blog.User.Image!;
                blogDetails.UserName = $"{blog.User.FName} {blog.User.LName}";
            }
            return blogDetails;
        }
        public async Task<IdentityResult> DeleteServiceAsync(ClaimsPrincipal User, int blogId)
        {
            User? user = await _userManager.GetUserAsync(User);
            Blog blog = await _blogRepository.GetBlogByIdAsync(blogId);
            if (blog == null)
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "BlogNotFound",
                    Description = "Blog Not Found."
                }
                   );
            bool checkUserPermission = blog.UserId == user!.Id;
            if (checkUserPermission)
                await _blogRepository.DeleteBlogAsync(blogId);
            else
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "BlogUpdateNotAllowed",
                    Description = "You Don`t have permission to delete this blog."
                }
                    );

            return IdentityResult.Success;
        }
    }
}
