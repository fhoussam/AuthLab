
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AuthLabOne.Bff.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : Controller
    {
        //[Authorize]
        //apprenelty old authorize doest inject RedirectUri when auto challenging oidc scheme,
        //so you have to use a traditional Challenge instruction
        [Route("login")]
        public IActionResult Index(string redirect)
        {
            if (User?.Identity?.IsAuthenticated != true)
                return Challenge(new AuthenticationProperties() { RedirectUri = $"/accounts/login?redirect={redirect}" }, "oidc");

            string path;

            var frontBaseUrl = "https://localhost:44450";

            switch (redirect)
            {
                case "customers":
                    path = "/customers";
                    break;

                case "orders":
                    path = "/orders";
                    break;

                case "fetch-data":
                    path = "/fetch-data";
                    break;

                default:
                    path = "/";
                    break;
            }

            var userInfo = new UserResponseDto()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Maximum Armor",
                Role = "Admin"
            };

            var serializerSettings = new JsonSerializerOptions();
            serializerSettings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            var jsonString = JsonSerializer.Serialize(userInfo, serializerSettings);
            HttpContext.Response.Cookies.Append("userProfile", jsonString, new CookieOptions() 
            { 
                Domain = ".localhost",
                Secure = false,
                SameSite = SameSiteMode.Lax,
                HttpOnly = false
            });

            return Redirect(frontBaseUrl + path + "/ups");
        }

        [Authorize]
        public IActionResult Logout()
        {
            var redirectUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}";
            return SignOut(new AuthenticationProperties() { RedirectUri = redirectUri }, "cookies", "oidc");
        }
    }

    public class UserResponseDto
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Id { get; set; }
    }
}