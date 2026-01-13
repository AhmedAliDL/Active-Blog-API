using Active_Blog_Service.Models;
using Active_Blog_Service_API.Context;
using Active_Blog_Service_API.Dto;
using Active_Blog_Service_API.Repositories.Contract;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Active_Blog_Service_API.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<Comment> GetCommentsOfBlogOrderByDateTimeWithoutUser(int blogId)
        {
            return _context.Comments
                .Where(c => c.BlogId == blogId)
                .OrderBy(c => c.CreatedDateTime)
                .AsQueryable();
        }
        public IQueryable<Comment> GetCommentsOfBlogOrderByDateTime(int blogId)
        {
            return _context.Comments
                .Where(c => c.BlogId == blogId)
                .OrderBy(c => c.CreatedDateTime)
                .Include(c => c.User)
                .AsQueryable();
        }

        public async Task AddCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }
        public async Task<Comment> GetSpecificComment(int blogId, int commentId)
        {
            return await GetCommentsOfBlogOrderByDateTime(blogId)!.FirstOrDefaultAsync(c => c.Id == commentId)!;
        }
        public async Task UpdateCommentAsync(Comment comment)
        {
            _context.Comments.Update(comment);

            await _context.SaveChangesAsync();
        }
        public async Task DeleteCommentAsync(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}
