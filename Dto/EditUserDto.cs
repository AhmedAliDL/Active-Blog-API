using Active_Blog_Service.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Active_Blog_Service_API.Dto
{
    public class EditUserDto
    {
        [MaxLength(30,ErrorMessage = "First Name must be between 3 and 30 characters.")]
        [MinLength(3,ErrorMessage = "First Name must be between 3 and 30 characters.")]
        public string? FName { get; set; }
        [MaxLength(30,ErrorMessage = "Last Name must be between 3 and 30 characters.")]
        [MinLength(3,ErrorMessage = "Last Name must be between 3 and 30 characters.")]
        public string? LName { get; set; }
        [MaxLength(11,ErrorMessage = "Phone Number must be 11 characters.")]
        [MinLength(11,ErrorMessage = "Phone Number must be 11 characters.")]
        public string? Phone { get; set; }
        public string? Address { get; set; }
        [RegularExpression(@"^[^\s@]+@[^\s@]+\.[cC][oO][mM]$", ErrorMessage = "Email must end with .com")]
        [DataType(DataType.EmailAddress)]
        [UniqueEmail(errorMessage: "This Email Address has been used before.")]
        public string? Email { get; set; }
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Confirmed Password field must match New Password field")]
        public string? ConfirmNewPassword { get; set; }
        [CheckImageExtension(errorMessage: "Invalid image file format. Only .jpg, .png, and .jpeg files are allowed.")]
        [CheckImageSize(maxSizeInMB: 5, errorMessage: "Image size must not exceed 5 MB")]
        public IFormFile? ImageFile { get; set; }
    }
}
