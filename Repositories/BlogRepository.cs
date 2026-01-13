using Active_Blog_Service.Models;
using Active_Blog_Service_API.Context;
using Active_Blog_Service_API.Dto;
using Active_Blog_Service_API.Repositories.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Active_Blog_Service_API.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly AppDbContext _context;

        public BlogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Blog>> GetAllBlogsAsync()
        {
            return await _context.Blogs.ToListAsync();
        }
        public async Task<Blog> GetBlogOfUserAsync(string userId, int blogId)
        {
            return await _context.Blogs.FirstOrDefaultAsync(b=> b.UserId == userId && b.Id == blogId)!;  
        }

        public async Task<Blog> GetBlogByIdAsync(int id)
        {
            return await _context.Blogs.Include(c=>c.User)!.FirstOrDefaultAsync(b => b.Id == id)!;
        }

        public async Task AddBlogAsync(Blog blog)
        {
           await  _context.Blogs.AddAsync(blog);
           await  _context.SaveChangesAsync();
        }
        public async Task UpdateBlogAsync(Blog blog)
        {
            _context.Blogs.Update(blog);

            await _context.SaveChangesAsync();
        }
        public async Task DeleteBlogAsync(int id)
        {
            Blog blog = await GetBlogByIdAsync(id);
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
        }

    }
}
