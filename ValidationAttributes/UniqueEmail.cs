using Active_Blog_Service.Models;
using Active_Blog_Service.Services.Contracts;
using Active_Blog_Service_API.Services.Contracts;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Active_Blog_Service.ValidationAttributes
{
    public class UniqueEmail : ValidationAttribute
    {
        private readonly string _errorMessage;
        public UniqueEmail(string errorMessage): base(errorMessage) 
        {
            _errorMessage = errorMessage;
            
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string? email = value as string;
            var userService = validationContext.GetService<IUserService>();
            bool emailFound =  userService!.CheckFoundOfEmail(email!);
            if (!emailFound)
                return ValidationResult.Success;
            return new ValidationResult(errorMessage: _errorMessage);

        }
    }
}
