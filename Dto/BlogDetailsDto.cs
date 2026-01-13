using Active_Blog_Service.Models;
using System.ComponentModel.DataAnnotations;

namespace Active_Blog_Service_API.Dto
{
    public class BlogDetailsDto
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string BlogImage { get; set; }
        public string BlogContent { get; set; }
        public DateOnly CreatedDate { get; set; }
        public string UserName { get; set; }
        public string UserImage {  get; set; }

        public virtual List<CommentDetailsDto> BlogComments { get; set; }

    }
}
