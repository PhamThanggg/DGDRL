using DGDiemRenLuyen.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace DGDiemRenLuyen.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("token")]
        public async Task<IActionResult> ExchangeCodeForToken([FromBody] TokenRequest request)
        {
            var keycloakSettings = _configuration.GetSection("Authentication");

            var formData = new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "client_id", keycloakSettings["ClientId"] },
            { "client_secret", keycloakSettings["ClientSecret"] },
            { "code", request.Code },
            { "redirect_uri", keycloakSettings["RedirectUri"] }
        };

            using var client = new HttpClient();
            var response = await client.PostAsync(keycloakSettings["TokenEndpoint"], new FormUrlEncodedContent(formData));
            var responseBody = await response.Content.ReadAsStringAsync();

            return Ok(JsonSerializer.Deserialize<object>(responseBody));
        }
    
    }
}
