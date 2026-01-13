using System.ComponentModel.DataAnnotations;

namespace Active_Blog_Service_API.Dto
{
    public class LoginDto
    {
        [RegularExpression(@"^[^\s@]+@[^\s@]+\.[cC][oO][mM]$", ErrorMessage = "Email must end with .com")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
    }
}
