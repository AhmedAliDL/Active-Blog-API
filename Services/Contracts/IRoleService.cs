using Active_Blog_Service.Models;
using Active_Blog_Service.ViewModels;
using Active_Blog_Service_API.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Active_Blog_Service.Services.Contracts
{
    public interface IRoleService : IScopedServiceMarker
    {
        Task<List<RoleDto>> GetAllRolesAsync();
        Task<IdentityResult> CreateRoleAsync(RoleDto roleDto);
        Task<IdentityResult> AssignRoleToUserAsync(User user, string roleName);
    }
}
