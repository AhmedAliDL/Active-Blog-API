using Active_Blog_Service.Services.Contracts;
using Active_Blog_Service.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Active_Blog_Service_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="admin")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        public RoleController(IRoleService roleService, IUserService userService)
        {
            _roleService = roleService;
            _userService = userService;
        }
        [HttpGet("role/index")]
        public async Task<IActionResult> Index()
        {
            var rolesDto = await _roleService.GetAllRolesAsync();
             return Ok(rolesDto); 
        }
        [HttpPost("role/add role")]
        public async Task<IActionResult> Add(RoleDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(roleDto);
            }
            var result = await _roleService.CreateRoleAsync(roleDto);

            if (result.Succeeded)
                return RedirectToAction("Index");
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Ok(roleDto);
        }
        [HttpPost("role/assign role")]
        public async Task<IActionResult> AssignRole([FromBody]AssignRoleDto assignRoleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(assignRoleDto);
            }
            var user = await _userService.GetUserByEmailAsync(assignRoleDto.UserEmail);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                return BadRequest(assignRoleDto);
            }
            var result = await _roleService.AssignRoleToUserAsync(user, assignRoleDto.RoleName);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Ok(assignRoleDto);
        }
    }
}
