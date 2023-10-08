using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using AuthLabOne.Identity.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthLabOne.Identity.Mine
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManageer;

        public ProfileService(UserManager<ApplicationUser> userManageer)
        {
            _userManageer = userManageer;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            if (!string.IsNullOrEmpty(sub))
            {
                var user = await _userManageer.FindByIdAsync(sub);
                var storedUserClaims = await _userManageer.GetClaimsAsync(user);
                context.IssuedClaims.AddEmailFrom(storedUserClaims);
                context.IssuedClaims.AddNameFrom(storedUserClaims);

                var claims = new List<Claim>();
                var externalLogins = await _userManageer.GetLoginsAsync(user);

                context.IssuedClaims.AddRange(claims);

                var roles = (await _userManageer.GetRolesAsync(user)).ToList();
                roles.ForEach(x => context.IssuedClaims.AddRole(x));

                //can be useful later
                //if (user != null && context.Caller == IdentityServerConstants.ProfileDataCallers.UserInfoEndpoint)
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            //var sub = context.Subject.GetSubjectId();
            //var user = await _userManageer.GetUserAsync(_httpContext.HttpContext.User);
            context.IsActive = true;
        }
    }
}
