using Active_Blog_Service.Models;
using Active_Blog_Service_API.Dto;
using Active_Blog_Service_API.Helpers;
using Active_Blog_Service_API.Repositories.Contract;
using Active_Blog_Service_API.Services.Contracts;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Active_Blog_Service_API.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly UserManager<User> _userManager;
        public CommentService(ICommentRepository commentRepository, UserManager<User> userManager)
        {
            _commentRepository = commentRepository;
            _userManager = userManager;
        }
        public async Task<List<Comment>> GetCommentsBelongsToBlog(int blogId)
        {
            return await _commentRepository.GetCommentsOfBlogOrderByDateTimeWithoutUser(blogId).ToListAsync();
        }
        public async Task AddCommentServiceAsync(ClaimsPrincipal User, HttpRequest request, AddCommentDto addCommentDto)
        {

            var user = await _userManager.GetUserAsync(User);

            var comment = new Comment
            {
                CommentContent = addCommentDto.CommentContent,
                UserId = user!.Id,
                BlogId = addCommentDto.BlogId
            };
            await _commentRepository.AddCommentAsync(comment);

        }
        public async Task<IdentityResult> UpdateCommentServiceAsync(ClaimsPrincipal User, EditCommentDto editCommentDto)
        {
            User? user = await _userManager.GetUserAsync(User);
            Comment oldComment = await _commentRepository.GetSpecificComment(editCommentDto.BlogId, editCommentDto.CommentId);
            if (oldComment == null)
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "CommentNotFound",
                    Description = "Comment Not Found."
                }
                    );
            bool checkUserPermission = oldComment.UserId == user!.Id;
            if (checkUserPermission)
            {
                oldComment.CommentContent = editCommentDto.CommentContent;

                await _commentRepository.UpdateCommentAsync(oldComment);
            }
            else
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "CommentNotAllowed",
                    Description = "You don`t have permission to edit this comment."
                });
            return IdentityResult.Success;
        }
        public async Task<IdentityResult> DeleteCommentServiceAsync(ClaimsPrincipal User, int blogId, int commentId)
        {
            User? user = await _userManager.GetUserAsync(User);
            Comment comment = await _commentRepository.GetSpecificComment(blogId, commentId);
            if (comment == null)
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "CommentNotFound",
                    Description = "Comment Not Found."
                }
                    );
            bool checkUserPermission = comment.UserId == user!.Id;
            if (checkUserPermission)
                await _commentRepository.DeleteCommentAsync(comment);
            else
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "CommentNotAllowed",
                    Description = "You don`t have permission to delete this comment."
                });
            return IdentityResult.Success;
        }
    }
}
