using Active_Blog_Service.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Active_Blog_Service_API.Dto
{
    public class EditBlogDto
    {
        public string? Title { get; set; }
        [MaxLength(50)]
        [MinLength(3)]
        public string? Category { get; set; }
        [CheckImageExtension(errorMessage: "Invalid image file format. Only .jpg, .png, and .jpeg files are allowed.")]
        [CheckImageSize(maxSizeInMB: 5, errorMessage: "Image size must not exceed 5 MB")]
        public IFormFile? Image { get; set; }
        [MinLength(50)]
        public string? BlogContent { get; set; }
    }

}
