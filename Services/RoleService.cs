using Active_Blog_Service.Models;
using Active_Blog_Service.Services.Contracts;
using Active_Blog_Service.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Active_Blog_Service.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            List<RoleDto> roleDto= await _roleManager.Roles.Select(role => new RoleDto
            {
                RoleName = role.Name!,
                RoleDescription = role.NormalizedName!
            }).ToListAsync();

            return roleDto;

        }
        public async Task<IdentityResult> CreateRoleAsync(RoleDto roleDto)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleDto.RoleName);
            if (roleExists)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Role '{roleDto.RoleName}' already exists."
                });
            }
            var role = new IdentityRole
            {
                Name = roleDto.RoleName,
                NormalizedName = roleDto.RoleDescription
            };
            var result = await _roleManager.CreateAsync(role);
            return result;
        }
        public async Task<IdentityResult> AssignRoleToUserAsync(User user, string roleName)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result;
        }

    }
}
