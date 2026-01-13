using Active_Blog_Service.Models;
using System.ComponentModel.DataAnnotations;

namespace Active_Blog_Service_API.Dto
{
    public class ContactDto
    {
        [MaxLength(40)]
        [MinLength(3)]
        public string Subject { get; set; }
        [MinLength(3)]
        public string Body { get; set; }

        public string FName { get; set; }
        public string LName { get; set; }
    }
}
