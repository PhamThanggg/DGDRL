using Azure;
using Azure.Core;
using DGDiemRenLuyen.Common;
using DGDiemRenLuyen.DTOs.HnueApiResponse;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DGDiemRenLuyen.Services.AuthService
{
    public class AuthService
    {
        private readonly IRoleAssignmentRepository _roleAssignmentRepository;
        private readonly ApiClientService _apiClientService;
        private readonly IConfiguration _configuration;

        public AuthService(
            IRoleAssignmentRepository roleAssignmentRepository,
            ApiClientService apiClientService,
            IConfiguration configuration)
        {
            _roleAssignmentRepository = roleAssignmentRepository;
            _apiClientService = apiClientService;
            _configuration = configuration;
        }

        public TokenResponse GenerateJwtToken(string tokenSSO)
        {
            var keycloakSettings = _configuration.GetSection("JwtSettings");

            var secretKey = keycloakSettings["SecretKey"];
            var issuer = keycloakSettings["Issuer"];
            var audience = keycloakSettings["Audience"];
            var accessTokenLifetime = keycloakSettings["AccessTokenExpirationMinutes"];
            var refreshTokenLifetime = keycloakSettings["RefreshTokenExpirationMinutes"];
          
            var roleDB = GetClaimFromToken(tokenSSO, ClaimConstant.ROLE_SSO);
            var preferred_username = GetClaimFromToken(tokenSSO, ClaimConstant.USER_ID);
            var email = GetClaimFromToken(tokenSSO, ClaimConstant.EMAIL);

            var role = GetRole(preferred_username) ?? roleDB;

            var claims = new Dictionary<string, object>
            {
                { ClaimConstant.ROLE_SSO, roleDB },
                { ClaimConstant.USER_ID, preferred_username },
                { ClaimConstant.EMAIL, email },
                { ClaimTypes.Role, role}
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtClaims = claims.Select(c => new Claim(c.Key, c.Value?.ToString() ?? string.Empty)).ToList();

            var accessTokenExpires = DateTime.UtcNow.AddMinutes(double.Parse(accessTokenLifetime));
            var refreshTokenExpires = DateTime.UtcNow.AddMinutes(double.Parse(refreshTokenLifetime));

            var accessToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: jwtClaims,
                expires: accessTokenExpires,
                signingCredentials: creds
            );

            var refreshToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: jwtClaims,
                expires: refreshTokenExpires,
                signingCredentials: creds
            );

            return new TokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken),
                AccessTokenExpiresAt = accessTokenExpires,
                RefreshTokenExpiresAt = refreshTokenExpires
            };
        }

        public JwtSecurityToken GetDecodedToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            return jsonToken;
        }

        public string GetClaimFromToken(string token, string claimType)
        {
            var decodedToken = GetDecodedToken(token);
            var claim = decodedToken?.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;

            return claim;
        }

        public string? GetRole(string userID)
        {
            RoleAssignment roleAssignment = _roleAssignmentRepository.GetById(userID);

            return roleAssignment?.Role;
        }

        public string? GetClassesAsync(string userID)
        {
            // Danh sách lớp giảng viên đảm nhiệm
            var classes = new HashSet<string>();

            // Dữ liệu gửi đi
            var requestData = new
            {
                ProfessorID = userID
            };

            // Gọi API để lấy danh sách giảng viên
            var apiTask = _apiClientService.GetDataFromApi<List<Professor>>(
                ApiRoute.baseUrl + ApiRoute.getProfessorList, requestData);

            Task.WhenAll(apiTask);

            var apiResponse = apiTask.Result;

            // Kiểm tra dữ liệu trả về
            if (apiResponse?.Data != null)
            {
                foreach (var professor in apiResponse.Data)
                {
                    if (professor.classes != null)
                    {
                        foreach (var classInfo in professor.classes)
                        {
                            classes.Add(classInfo.classStudentID);
                        }
                    }
                }
            }

            return classes.Count > 0 ? string.Join(",", classes) : null;
        }

    }
}
