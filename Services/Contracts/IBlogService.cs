using Active_Blog_Service.Models;
using Active_Blog_Service_API.Dto;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Active_Blog_Service_API.Services.Contracts
{
    public interface IBlogService : IScopedServiceMarker
    {
        Task<List<Blog>> GetAllBlogsServiceAsync();
        Task<Blog> GetBlogServiceAsync(int id);
        Task AddBlogServiceAsync(ClaimsPrincipal User, HttpRequest request, AddBlogDto addBlogDto);
        Task<IdentityResult> UpdateBlogServiceAsync(ClaimsPrincipal User, int id, EditBlogDto newBlog);
        Task<IdentityResult> DeleteServiceAsync(ClaimsPrincipal User, int blogId);
        Task<BlogDetailsDto> ShowBlogServiceAsync(ClaimsPrincipal User, HttpRequest request, int id);
    }
}
