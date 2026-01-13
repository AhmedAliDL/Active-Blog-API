using Active_Blog_Service_API.Dto;
using Active_Blog_Service_API.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

public interface IAccountService : IScopedServiceMarker
{
    Task<IdentityResult> RegisterUserAsync(RegisterDto registerDto);
    Task<object> LoginUserAsync(LoginDto loginDto);
    Task<IdentityResult> EditUserAsync(ClaimsPrincipal user, EditUserDto editUserDto);
}

