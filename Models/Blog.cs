using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Active_Blog_Service.Models
{
    public class Blog
    {
        public int Id { get; set; }
        [MaxLength(50, ErrorMessage = "Blog Title Must be between 3 and 50 character")]
        [MinLength(3, ErrorMessage = "Blog Title Must be between 3 and 50 character")]
        public string Title { get; set; }
        [MaxLength(50, ErrorMessage = "Blog Category Must be between 3 and 50 character")]
        [MinLength(3, ErrorMessage = "Blog Category Must be between 3 and 50 character")]
        public string Category { get; set; }
        public string? Image { get; set; } 
        [MinLength(50,ErrorMessage ="Blog Content allow at least 50 character")]
        public string BlogContent { get; set; }
        public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string UserId {  get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }

        public virtual User User { get; set; }

    }
}
