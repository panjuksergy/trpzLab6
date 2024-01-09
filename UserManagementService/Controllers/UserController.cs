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
    public class UserController : BaseController
    {
        private UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("update")]
        public async Task<IdentityResult> Update(UpdateUserDto updateUser)
        {
            var userToBeUpdated = await _userManager.FindByIdAsync(UserId.ToString());

            userToBeUpdated.Email = userToBeUpdated.Email;
            userToBeUpdated.UserName = userToBeUpdated.UserName;
            userToBeUpdated.FirstName = updateUser.FirstName;
            userToBeUpdated.LastName = updateUser.LastName;

            var result = await _userManager.UpdateAsync(userToBeUpdated);
            return result;
        }

        [HttpPost("changepassword")]
        public async Task<IdentityResult> ChangePassword(ChangePasswordDto changePasswordModel)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            var result = await _userManager.ChangePasswordAsync
                (user, changePasswordModel.CurrentPassword, changePasswordModel.NewPassword);

            return result;
        }
    }
}
