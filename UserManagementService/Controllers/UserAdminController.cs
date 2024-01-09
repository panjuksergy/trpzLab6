using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SparkSwim.Core.Models;
using UserManagementService.Controllers.Base;
using UserManagementService.Models;

namespace UserManagementService.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "Admin")]
    public class UserAdminController : BaseController
    {
        private UserManager<AppUser> _userManager;

        public UserAdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("changepassword")]
        public async Task<IdentityResult> ChangePassword(ChangePasswordDto changePasswordModel)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            var result = await _userManager.ChangePasswordAsync
                (user, changePasswordModel.CurrentPassword, changePasswordModel.NewPassword);

            return result;
        }

        [HttpPost("remove")]
        [Authorize(Roles = "Admin")]
        public async Task<IdentityResult> RemoveByUserName(string userName)
        {
            var userToBeDeleted = await _userManager.FindByNameAsync(userName);
            var result = await _userManager.DeleteAsync(userToBeDeleted);

            return result;
        }

        [HttpPost("addrole")]
        [Authorize(Roles = "Admin")]
        public async Task<IdentityResult> AddRole(AddRoleToUserDto addRoleToUser)
        {
            var user = await _userManager.FindByNameAsync(addRoleToUser.UserName);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError() { Description = $"User {addRoleToUser.UserName} was not found." });
            }
            var result = await _userManager.AddToRoleAsync(user, addRoleToUser.RoleName);

            return result;
        }

        [HttpPost("removerole")]
        [Authorize(Roles = "Admin")]
        public async Task<IdentityResult> RemoveRole(AddRoleToUserDto addRoleToUser)
        {
            var user = await _userManager.FindByNameAsync(addRoleToUser.UserName);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError() { Description = $"User {addRoleToUser.UserName} was not found." });
            }
            var result = await _userManager.RemoveFromRoleAsync(user, addRoleToUser.RoleName);

            return result;
        }
        
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<AppUser>> GetAll()
        {
            var result = _userManager.Users.AsEnumerable();
            return result;
        }

        [HttpGet("get")]
        public async Task<AppUser> GetByName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user;
        }
    }
}
