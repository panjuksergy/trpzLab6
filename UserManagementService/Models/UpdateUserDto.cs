using System.ComponentModel.DataAnnotations;

namespace UserManagementService.Models
{
    public class UpdateUserDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
