using System.ComponentModel.DataAnnotations;

namespace Active_Blog_Service.ValidationAttributes
{
    public class CheckImageExtension : ValidationAttribute
    {
        private readonly string _errorMessage;
        public CheckImageExtension(string errorMessage) 
        {
          _errorMessage = errorMessage;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return new ValidationResult(_errorMessage);
                }
            }
            return ValidationResult.Success;
        }

    }
}
