using IdentityModel.OidcClient;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AuthLabOne.Bff.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : Controller
    {
        private readonly OidcClient _oidcClient;

        public AccountsController(OidcClient oidcClient)
        {
            _oidcClient = oidcClient;
        }

        [HttpGet]
        [Route("user-info")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                if (string.IsNullOrEmpty(accessToken))
                    return NoContent();

                var result = await _oidcClient.GetUserInfoAsync(accessToken);
                var claimList = result.Claims.Select(x => new
                {
                    x.Type,
                    x.Value
                });

                var response = new UserResponseDto()
                {
                    Id = claimList.Single(x => x.Type == JwtClaimTypes.Subject).Value,
                    Name = claimList.Single(x => x.Type == JwtClaimTypes.Name).Value,
                    Role = claimList.Any(x => x.Type == JwtClaimTypes.Email) ? "Customer" : claimList.Single(x => x.Type == JwtClaimTypes.Role).Value,
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }

    public class UserResponseDto
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Id { get; set; }
    }
}
