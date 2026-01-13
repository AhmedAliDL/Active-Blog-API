using Active_Blog_Service.Models;
using Active_Blog_Service_API.Dto;

namespace Active_Blog_Service_API.Repositories.Contract
{
    public interface IBlogRepository : IScoppedRepositoryMarker
    {
        Task<List<Blog>> GetAllBlogsAsync();
        Task<Blog> GetBlogByIdAsync(int id);
        Task<Blog> GetBlogOfUserAsync(string userId, int blogId);
        Task AddBlogAsync(Blog blog);
        Task UpdateBlogAsync(Blog blog);
        Task DeleteBlogAsync(int id);
    }
}
