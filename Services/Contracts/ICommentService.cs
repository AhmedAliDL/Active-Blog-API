using Active_Blog_Service.Models;
using Active_Blog_Service_API.Dto;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Active_Blog_Service_API.Services.Contracts
{
    public interface ICommentService : IScopedServiceMarker
    {
        Task<List<Comment>> GetCommentsBelongsToBlog(int blogId);
        Task AddCommentServiceAsync(ClaimsPrincipal User, HttpRequest request, AddCommentDto addCommentDto);
        Task<IdentityResult> DeleteCommentServiceAsync(ClaimsPrincipal User, int blogId, int commentId);
        Task<IdentityResult> UpdateCommentServiceAsync(ClaimsPrincipal User, EditCommentDto editCommentDto);
    }
}
