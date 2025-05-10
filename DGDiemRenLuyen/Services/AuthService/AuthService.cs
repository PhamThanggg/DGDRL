using DGDiemRenLuyen.Common;
using DGDiemRenLuyen.DTOs.HnueApiResponse;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.DTOs.Responses.Students;
using DGDiemRenLuyen.Extentions;
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
        private readonly IActiveTokenRepository _activeTokenRepository;
        private readonly ApiClientService _apiClientService;
        private readonly IConfiguration _configuration;

        DateTime accessTokenExpires;
        DateTime refreshTokenExpires;

        public AuthService(
            IRoleAssignmentRepository roleAssignmentRepository,
            IActiveTokenRepository activeTokenRepository,
            ApiClientService apiClientService,
            IConfiguration configuration)
        {
            _roleAssignmentRepository = roleAssignmentRepository;
            _activeTokenRepository = activeTokenRepository;
            _apiClientService = apiClientService;
            _configuration = configuration;
        }

        public TokenResponse GenerateJwtToken(string tokenSSO)
        {
            var roleDB = GetClaimFromToken(tokenSSO, ClaimConstant.ROLE_SSO);
            var preferred_username = GetClaimFromToken(tokenSSO, ClaimConstant.USER_ID);
            var email = GetClaimFromToken(tokenSSO, ClaimConstant.EMAIL);

            var role = GetRole(preferred_username) ?? roleDB;

            var claimAccess = new Dictionary<string, object>
            {
                { ClaimConstant.ROLE_SSO, roleDB },
                { ClaimConstant.USER_ID, preferred_username },
                { ClaimConstant.EMAIL, email },
                { ClaimTypes.Role, role},
            };

            var claimRefresh = new Dictionary<string, object>
            {
                { ClaimConstant.ROLE_SSO, roleDB },
                { ClaimConstant.USER_ID, preferred_username },
                { ClaimConstant.EMAIL, email },
                { ClaimTypes.Role, roleDB},
            };

            var accessToken = GenerateToken(claimAccess, role, preferred_username, true);
            var refreshToken = GenerateToken(claimRefresh, role, preferred_username, false);

            return new TokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken),
                AccessTokenExpiresAt = accessTokenExpires,
                RefreshTokenExpiresAt = refreshTokenExpires
            };
        }

        private JwtSecurityToken GenerateToken(
            Dictionary<string, object> claims
            , string role, string preferred_username
            , bool isAccessToken)
        {
            var keycloakSettings = _configuration.GetSection("JwtSettings");

            var secretKey = keycloakSettings["SecretKey"];
            var issuer = keycloakSettings["Issuer"];
            var audience = keycloakSettings["Audience"];
            var accessTokenLifetime = keycloakSettings["AccessTokenExpirationMinutes"];

            DateTime expiresAt = DateTime.UtcNow.AddMinutes(double.Parse(accessTokenLifetime));
            accessTokenExpires = expiresAt;

            if (isAccessToken)
            {
                if (role == RoleConstants.CBL || role == RoleConstants.SV)
                {
                    var res = GetData<StudentResponse>(preferred_username, ApiRoute.getDetailStudent);
                    claims.Add(ClaimConstant.DEPARTMENT, res.DepartmentID);
                    claims.Add(ClaimConstant.CLASS, res.ClassStudentID);
                }

                if (role == RoleConstants.GV || role == RoleConstants.TK)
                {
                    var res = GetData<Professor>(preferred_username, ApiRoute.getDetailProfessor);
                    claims.Add(ClaimConstant.DEPARTMENT, res.departmentID);
                }
                claims.Add(ClaimConstant.TYP, "access");
            }
            else
            {
                Guid jti = Guid.NewGuid();
                claims.Add(ClaimConstant.JTI, jti);
                claims.Add(ClaimConstant.TYP, "refresh");
                var refreshTokenLifetime = keycloakSettings["RefreshTokenExpirationMinutes"];
                expiresAt = DateTime.UtcNow.AddMinutes(double.Parse(refreshTokenLifetime));
                refreshTokenExpires = expiresAt;

                ActiveToken activeToken = new ActiveToken
                {
                    Id = jti,
                    ObjectId = preferred_username,
                    ExpiresAt = expiresAt
                };
                _activeTokenRepository.Add(activeToken);
                _activeTokenRepository.Save();
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtClaims = claims
                .Select(c => new Claim(c.Key, c.Value?.ToString() ?? string.Empty))
                .ToList();

            return new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: jwtClaims,
                expires: expiresAt,
                signingCredentials: creds
            );
        }

        public TokenResponse RefreshJwtToken(string tokenSSO)
        {
            Guid jti = CheckRefresh(tokenSSO);

            ActiveToken activeToken = _activeTokenRepository.GetById(jti);

            if(activeToken == null)
            {
                throw new BaseException { Messages = ValidationKeyWords.VALID_TOKEN };
            }

            TokenResponse tokenResponse = GenerateJwtToken(tokenSSO);
            
            if(tokenResponse != null)
            {
                _activeTokenRepository.Delete<Guid>(jti);
                _activeTokenRepository.Save();
            }

            return tokenResponse;
        }

        public string Logout(string tokenSSO)
        {
            Guid jti = CheckRefresh(tokenSSO);
            _activeTokenRepository.Delete<Guid>(jti);
            _activeTokenRepository.Save();

            return "Đăng xuất thành công";
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

        public HashSet<string> GetClasses(string userID, string termID, string yearStudy)
        {
            // Danh sách lớp giảng viên đảm nhiệm
            var classes = new HashSet<string>();

            var requestData = new
            {
                ProfessorID = userID,
                TermID = termID,
                YearStudy = yearStudy,
            };

            // Gọi API để lấy danh sách giảng viên
            var apiTask = _apiClientService.GetDataFromApi<List<Professor>>(
                ApiRoute.baseUrl + ApiRoute.getProfessorList, requestData);

            Task.WhenAll(apiTask);

            var apiResponse = apiTask.Result;

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

            return classes;
        }

        public TResponse? GetData<TResponse>(string userID, string endpoint)
        {
            var apiTask = _apiClientService.GetDataFromApi<TResponse>(
                ApiRoute.baseUrl + endpoint + userID);

            Task.WhenAll(apiTask);

            return apiTask.Result.Data;
        }

        public Guid CheckRefresh(string token)
        {
            var keycloakSettings = _configuration.GetSection("JwtSettings");

            var secretKey = keycloakSettings["SecretKey"];
            var issuer = keycloakSettings["Issuer"];
            var audience = keycloakSettings["Audience"];

            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,       
                ValidAudience = audience,       
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };

            ClaimsPrincipal principal;
            SecurityToken validatedToken;

            try
            {
                principal = handler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch
            {
                throw new BaseException { Messages = ValidationKeyWords.VALID_TOKEN };
            }

            var tokenType = principal.Claims.FirstOrDefault(c => c.Type == ClaimConstant.TYP)?.Value;
            var jti = principal.Claims.FirstOrDefault(c => c.Type == ClaimConstant.JTI)?.Value;

            if (tokenType != "refresh")
            {
                throw new BaseException { Messages = ValidationKeyWords.VALID_TOKEN };
            }

            return Guid.Parse(jti);
        }


    }
}
