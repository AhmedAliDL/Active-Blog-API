namespace Active_Blog_Service_API.Dto
{
    public class EditCommentDto
    {
        public string CommentContent { get; set; }
        public int BlogId { get; set; }
        public int CommentId { get; set; }
    }
}
