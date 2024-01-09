namespace UserManagementService.Models
{
    public class ChangePasswordDto
    {
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

    }
}
