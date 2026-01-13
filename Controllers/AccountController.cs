using Active_Blog_Service_API.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Active_Blog_Service_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("user/register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.RegisterUserAsync(registerDto);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
            }
            return BadRequest(ModelState);
        }
        [HttpPost("user/login")]
        public async Task<IActionResult> Login([FromBody]LoginDto loginDto)
        {
            if(ModelState.IsValid)
            {
                var result = await _accountService.LoginUserAsync(loginDto);
                if (result != null)
                {
                    return Ok(result);
                }
                return Unauthorized("Email Or Password Not correct!");
            }
            return BadRequest(ModelState);
        }
        [Authorize]
        [HttpPut("user/edit")]
        public async Task<IActionResult> EditUser([FromBody] EditUserDto editUserDto)
        {
            var user = User;
            if (ModelState.IsValid)
            {
                var result = await _accountService.EditUserAsync(User, editUserDto);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
            }
            return BadRequest(ModelState);
        }
    }
}
