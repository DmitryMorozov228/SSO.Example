using System;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace SSO.Example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private const string PublicMessageData = "This is public data";
        private const string PrivateMessageData = "This is private data";

        private readonly IConfiguration _configuration;

        public AuthorizationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("public")]
        public ActionResult<string> GetPublicData()
        {
            return PublicMessageData;
        }

        [HttpPost]
        [Route("login")]
        public async Task<AccessTokenResponse> LoginAsync()
        {
            var client = new AuthenticationApiClient(new Uri(_configuration[ConfigurationSettings.OpenIdConnectAuthority]));

            var token = await client.GetTokenAsync(new ClientCredentialsTokenRequest
            {
                Audience = _configuration[ConfigurationSettings.OpenIdConnectAudience],
                ClientId = _configuration[ConfigurationSettings.OpenIdConnectClientId],
                ClientSecret = _configuration[ConfigurationSettings.OpenIdConnectClientSecret]
            });

            return token;
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
