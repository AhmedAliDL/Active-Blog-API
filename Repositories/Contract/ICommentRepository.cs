using Active_Blog_Service.Models;
using Active_Blog_Service_API.Dto;

namespace Active_Blog_Service_API.Repositories.Contract
{
    public interface ICommentRepository : IScoppedRepositoryMarker
    {
        IQueryable<Comment> GetCommentsOfBlogOrderByDateTime(int blogId);
        IQueryable<Comment> GetCommentsOfBlogOrderByDateTimeWithoutUser(int blogId);
        Task AddCommentAsync(Comment comment);
        Task<Comment> GetSpecificComment(int blogId, int commentId);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(Comment comment);

    }
}
