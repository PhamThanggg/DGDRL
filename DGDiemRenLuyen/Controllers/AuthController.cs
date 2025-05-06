using DGDiemRenLuyen.DTOs.Requests;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Services.AuthService;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace DGDiemRenLuyen.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(AuthService authService,IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("token")]
        public async Task<TokenResponse> ExchangeCodeForToken([FromBody] TokenRequest request)
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
                // return Ok(JsonSerializer.Deserialize<object>(responseBody));
            }

            var tokenSSO = tokenResponse["access_token"].ToString();

            return _authService.GenerateJwtToken(tokenSSO);
        }

       /* [HttpPost("refresh")]
        public async Task<TokenResponse> Refresh([FromBody] string refreshToken)
        {
            var storedToken = await _tokenService.GetValidRefreshTokenAsync(refreshToken);
            if (storedToken == null)
                return Unauthorized("Invalid or expired refresh token");

            await _tokenService.RevokeTokenAsync(refreshToken); // revoke old one

            var userId = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken).Subject;
            var newAccessToken = _tokenService.GenerateAccessToken(userId);
            var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync(userId);

            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }*/
    }
}
