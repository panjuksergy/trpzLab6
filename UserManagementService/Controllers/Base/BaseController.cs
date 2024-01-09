using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UserManagementService.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        public Guid UserId => !User.Identity.IsAuthenticated
            ? Guid.Empty
            : Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        public string UserEmail => !User.Identity.IsAuthenticated
            ? string.Empty
            : User.FindFirst(ClaimTypes.Email).Value;
    }
}
