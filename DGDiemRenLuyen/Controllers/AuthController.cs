using DGDiemRenLuyen.DTOs.Requests;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Services.AuthService;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DGDiemRenLuyen.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly TokenService _tokenService;
        private readonly RefreshTokenService _refreshTokenService;
        private readonly LogoutService _logoutService;
        private readonly IConfiguration _configuration;

        public AuthController(
            IConfiguration configuration,
            TokenService tokenService,
            RefreshTokenService refreshTokenService,
            LogoutService logoutService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _logoutService = logoutService;
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

            var tokenResponse = JsonSerializer.Deserialize<Dictionary<string, Object>>(responseBody);

            if (tokenResponse == null && !tokenResponse.ContainsKey("access_token"))
            {
                throw new BaseException { Messages = "Đăng nhập thất bại vui lòng thử lại sau" };
            }

            var tokenSSO = tokenResponse["access_token"].ToString();

            var result = _tokenService.Process(tokenSSO);

            return Ok(result);
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] TokenRequest token)
        {
            var result = _refreshTokenService.Process(token.Code);

            return Ok(result);
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromBody] TokenRequest token)
        {
            var result = _logoutService.Process(token.Code);
            result.Messages = result.Data.ToString();
            result.Data = null;
           
            return Ok(result);
        }
    }
}
