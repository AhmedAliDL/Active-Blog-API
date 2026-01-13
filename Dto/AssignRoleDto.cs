using Microsoft.AspNetCore.Identity;

namespace Active_Blog_Service.ViewModels
{
    public class AssignRoleDto
    {
        public string UserEmail { get; set; }
        public string RoleName { get; set; } 
    }
}
