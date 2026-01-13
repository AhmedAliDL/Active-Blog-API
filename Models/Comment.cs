using System.ComponentModel.DataAnnotations;

namespace Active_Blog_Service.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [MaxLength(100,ErrorMessage = "Comment Content Must be between 3 and 100 character")]
        [MinLength(3, ErrorMessage = "Comment Content Must be between 3 and 100 character")]
        public string CommentContent { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public int BlogId { get; set; }
        public string UserId { get; set; }

        public virtual Blog Blog { get; set; }
        public virtual User User { get; set; }

    }
}
