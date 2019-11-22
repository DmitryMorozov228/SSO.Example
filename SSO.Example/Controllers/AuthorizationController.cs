using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SSO.Example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private const string PublicMessageData = "This is public data";
        private const string PrivateMessageData = "This is private data";

        [HttpGet]
        [Route("public")]
        public ActionResult<string> GetPublicData()
        {
            return PublicMessageData;
        }

        [HttpPost]
        [Route("login")]
        public async Task<string> LoginAsync()
        {
            await HttpContext.ChallengeAsync("Auth0");
            return await HttpContext.GetTokenAsync("access_token");
        }
        
        [HttpGet]
        [Authorize(Policy = "AuthorizedUser")]
        [Route("private")]
        public ActionResult<string> GetPrivateData()
        {
            return PrivateMessageData;
        }
    }
}
