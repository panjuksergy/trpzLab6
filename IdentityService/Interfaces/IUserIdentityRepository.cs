using IdentityService.Models;
using SparkSwim.Core.Models;

namespace IdentityService.Interfaces
{
    public interface IUserIdentityRepository
    {
        public Task<AppUser> ValidateUserByEmail(UserLoginDto userLoginDto);
    }
}
