using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SparkSwim.Core.Models;
using UserManagementService.Controllers.Base;
using UserManagementService.Models;

namespace UserManagementService.Controllers
{
    public class UserAnonymousController : BaseController
    {
        private UserManager<AppUser> _userManager;

        public UserAnonymousController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IdentityResult> Register(RegisterUserDto registerUserModel)
        {
            var appUser = new AppUser()
            {
                UserName = registerUserModel.UserName,
                Email = registerUserModel.Email,
                FirstName = registerUserModel.FirstName,
                LastName = registerUserModel.LastName
            };
            var result = await _userManager.CreateAsync(appUser, registerUserModel.Password);
            return result;
        }
    }
}
