using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementService.Controllers.Base;

namespace UserManagementService.Controllers
{
    public class HealthController : BaseController
    {
        [HttpGet("zalupa")]
        public async Task<IActionResult> Get()
        {
            return Ok("IM ALIVVEE");
        }

        [HttpGet("admincheck")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AdminCheck()
        {
            return Ok("User is admin");
        }
    }
}
