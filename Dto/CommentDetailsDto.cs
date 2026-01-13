namespace Active_Blog_Service_API.Dto
{
    public class CommentDetailsDto
    {
        public string UserName { get; set; }
        public string CommentContent { get; set; }
        public string? UserImage { get; set; }
        public DateTime CommentDate { get; set; }
    }
}
