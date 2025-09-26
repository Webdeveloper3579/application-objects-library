using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using AOL_Portal.Data;

namespace AOL_Portal.Configuration
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<AolApplicationUser>
    {
        private readonly RoleManager<AolUserRole> roleManager;
        private readonly UserManager<AolApplicationUser> userManager;
        public ApplicationUserClaimsPrincipalFactory(UserManager<AolApplicationUser> userManager, RoleManager<AolUserRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, options)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AolApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user).ConfigureAwait(false);

            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            identity.AddClaim(new Claim("displayname", $"{user.FirstName} {user.Surname}"));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email.ToString()));
            if (user.IsSiteAdmin)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "SiteAdmin"));
            }
            identity.AddClaim(new Claim("isSiteadmin", user.IsSiteAdmin.ToString()));

            return identity;
        }
    }
}
