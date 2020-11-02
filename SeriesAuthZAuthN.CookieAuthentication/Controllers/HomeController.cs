using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SeriesAuthZAuthN.CookieAuthentication.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IConfiguration configuration;

        public HomeController(IConfiguration config) => configuration = config;

        public ActionResult Index() => Ok("Olá do index");


        [Authorize(Policy = "Admin")]
        public ActionResult SecretApi() => Ok("Secret API");


        [Authorize]
        public ActionResult Claims() => Ok(User.Claims.Select(claim => new { claim.Type, claim.Value }));

        public async Task<ActionResult> Autentication()
        {
            var identity01 = new ClaimsIdentity(configuration["DefaultSchema"]);
            identity01.AddClaim(new Claim(ClaimTypes.NameIdentifier, "Anderson Serra"));
            identity01.AddClaim(new Claim(ClaimTypes.Email, "anderson@gmail.com"));
            identity01.AddClaim(new Claim(ClaimTypes.Webpage, "anderson.com.br"));

            identity01.AddClaim(new Claim(ClaimTypes.Role, "SecretRole"));
            identity01.AddClaim(new Claim(ClaimTypes.Role, "Basic"));

            var principal = new ClaimsPrincipal(identity01);

            await HttpContext.SignInAsync(principal);

            return Redirect("/home");
        }
    }
}
