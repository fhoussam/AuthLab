using IdentityModel;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AuthLabOne.Identity.Mine
{
    public static class Extensions
    {
        private static void AddClaimFrom(IList<Claim> claims, IEnumerable<Claim> from, string claimType)
        {
            var claimValue = from.FirstOrDefault(x => x.Type == claimType)?.Value;
            if (!string.IsNullOrEmpty(claimValue))
            {
                claims.Add(new Claim(claimType, claimValue));
            }
        }

        public static void AddEmailFrom(this IList<Claim> claims, IEnumerable<Claim> from)
        {
            AddClaimFrom(claims, from, JwtClaimTypes.Email);
        }

        public static void AddPictureFrom(this IList<Claim> claims, IEnumerable<Claim> from)
        {
            AddClaimFrom(claims, from, JwtClaimTypes.Picture);
        }

        public static void AddNameFrom(this IList<Claim> claims, IEnumerable<Claim> from)
        {
            AddClaimFrom(claims, from, JwtClaimTypes.Name);
        }

        public static void AddRole(this IList<Claim> claims, string roleName)
        {
            if (claims != null)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, roleName));
            }
        }
    }
}
