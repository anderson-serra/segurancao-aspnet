using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SeriesAuthZAuthN.IdentityAuthentication.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SeriesAuthZAuthN.IdentityAuthentication.Identity
{
    public class AdditionalUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public AdditionalUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options) { }

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);

            var identity = principal.Identity as ClaimsIdentity;

            var claims = new List<Claim>();
            if (user.IsAdmin)
                claims.Add(new Claim(ClaimTypes.Role, "AdminBlaster"));
            else
                claims.Add(new Claim(ClaimTypes.Role, "UsuarioBlaster"));

            identity.AddClaims(claims);

            return principal;
        }
    }
}
