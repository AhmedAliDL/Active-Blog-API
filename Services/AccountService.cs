using Active_Blog_Service.Exceptions;
using Active_Blog_Service.Models;
using Active_Blog_Service_API.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Active_Blog_Service_API.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
       
        public AccountService(UserManager<User> userManager)
        {
            _userManager = userManager;
           
        }
        public async Task<IdentityResult> RegisterUserAsync(RegisterDto registerDto)
        {
            var user = new User
            {
                FName = registerDto.FName,
                Email = registerDto.Email,
                LName = registerDto.LName,
                PhoneNumber = registerDto.Phone,
                Address = registerDto.Address,
            };
            user.UserName = registerDto.Email;

            try
            {
                if (registerDto.ImageFile != null && registerDto.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserImages");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(registerDto.ImageFile!.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await registerDto.ImageFile.CopyToAsync(stream);
                    }

                    user.Image = $"/UserImages/{filePath}";
                }
            }
            catch (IOException ex)
            {
                throw new ImageUploadException("Image upload failed. Please try again.", ex);
            }

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if(result.Succeeded)
                await _userManager.AddToRoleAsync(user, "user");

            return result;
        }
        public async Task<object> LoginUserAsync(LoginDto loginDto)
        {

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user != null)
            {
                var found = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (found)
                {

                    var config = WebApplication.CreateBuilder().Configuration;
                    //create token
                    var claims = new List<Claim>
                        {
                            new(ClaimTypes.NameIdentifier , user.Id),
                            new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                        };

                    //get role
                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        claims.Add(new(ClaimTypes.Role, role));
                    }

                    SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecurityKey"]!));

                    var signingCred = new SigningCredentials(
                        algorithm: SecurityAlgorithms.HmacSha256,
                        key: key
                        );
                    var token = new JwtSecurityToken(
                        issuer: config["JWT:issuer"],
                        audience: config["JWT:audience"],
                        claims: claims,
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: signingCred
                        );

                    
                    return new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    };
                }
            }
            return null!;


        }

        public async Task<IdentityResult> EditUserAsync(ClaimsPrincipal user, EditUserDto editUserDto)
        {
            var appUser = await _userManager.GetUserAsync(user);
            if (appUser != null)
            {
                appUser.Email = editUserDto.Email ?? appUser.Email;
                appUser.Address = editUserDto.Address ?? appUser.Address;
                appUser.PhoneNumber = editUserDto.Phone ?? appUser.PhoneNumber;
                appUser.FName = editUserDto.FName ?? appUser.FName;
                appUser.LName = editUserDto.LName ?? appUser.LName;
                if (editUserDto.NewPassword != null && editUserDto.CurrentPassword != null)
                {
                    var result = await _userManager.ChangePasswordAsync(appUser, editUserDto.CurrentPassword, editUserDto.NewPassword);
                    if (!result.Succeeded)
                        return result;

                }

                if (editUserDto.ImageFile != null && editUserDto.ImageFile.Length > 0)
                {
                    try
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserImages");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(editUserDto.ImageFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await editUserDto.ImageFile.CopyToAsync(stream);
                        }
                        appUser.Image = $"/UserImages/{filePath}";
                    }
                    catch (IOException ex)
                    {
                        throw new ImageUploadException("Image upload failed. Please try again.", ex);
                    }
                }
                var updateResult = await _userManager.UpdateAsync(appUser);

                return updateResult;
            }
            return IdentityResult.Failed(
                        new IdentityError
                        {
                            Description = "User not found."
                        }
                );
        }
    }

}


