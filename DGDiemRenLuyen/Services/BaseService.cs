using DGDiemRenLuyen.Common;
using DGDiemRenLuyen.DTOs.Responses;
using System.Security.Claims;

namespace DGDiemRenLuyen.Services
{
    public abstract class BaseService<T1, T2>
    {
        public T1 _dataRequest;
        public T2 _dataResponse;
        public T2 _exceptionDataResponse;
        public ApiResponse<T2> _responseResult;
        private IHttpContextAccessor _httpContextAccessor;

        protected BaseService(
            IHttpContextAccessor httpContextAccessor,
            string successMessageDefault = "")
        {
            _httpContextAccessor = httpContextAccessor;
            _responseResult = new ApiResponse<T2>()
            {
                StatusCode = StatusCodes.Status200OK.ToString(),
                Messages = successMessageDefault
            };
        }

        public IHttpContextAccessor HttpContextAccessor
        {
            get
            {
                return _httpContextAccessor;
            }
        }

        public string? UserID
        {
            get
            {
                string userID = GetClaimValue(HttpContextAccessor, ClaimConstant.USER_ID);
                if (!string.IsNullOrEmpty(userID))
                {
                    return userID;
                }
                return null;
            }
        }

        public string? DepartmentID
        {
            get
            {
                string department = GetClaimValue(HttpContextAccessor, ClaimConstant.DEPARTMENT);
                if (!string.IsNullOrEmpty(department))
                {
                    return department;
                }
                return null;
            }
        }

        public string? ClassStudentID
        {
            get
            {
                string classID = GetClaimValue(HttpContextAccessor, ClaimConstant.CLASS);
                if (!string.IsNullOrEmpty(classID))
                {
                    return classID;
                }
                return null;
            }
        }

        public string? Role
        {
            get
            {
                string role = GetClaimValue(HttpContextAccessor, ClaimConstant.ROLE);
                if (!string.IsNullOrEmpty(role))
                {
                    return role;
                }
                return null;
            }
        }

        private string? GetClaimValue(IHttpContextAccessor httpContextAccessor, string claimName)
        {
            try
            {
                var identity = (ClaimsIdentity)httpContextAccessor.HttpContext.User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var claimsDetail = claims.FirstOrDefault(c => c.Type == claimName);
                if (claimsDetail != null)
                {
                    return claimsDetail.Value;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public abstract void P1GenerateObjects();

        /// Check validate
        public abstract void P2PostValidation();

        /// Access DB 
        public abstract void P3AccessDatabase();

        /// data response 
        public abstract void P4GenerateResponseData();

        public ApiResponse<T2> Process(T1 dataRequest = default(T1))
        {
            try
            {
                _dataRequest = dataRequest;
                P1GenerateObjects();
                P2PostValidation();
                P3AccessDatabase();
                P4GenerateResponseData();
                _responseResult.Data = _dataResponse;
                return _responseResult;
            }
            catch (BaseException ex)
            {
                return new ApiResponse<T2>
                {
                    StatusCode = ex.StatusCode,
                    Messages = ex.Messages,
                    MessagesDetails = ex.MessagesDetails,
                    Data = _exceptionDataResponse,
                };
            }
            catch (Exception ex)
            {
                // Exception Log
                return new ApiResponse<T2>
                {
                    StatusCode = StatusCodes.Status500InternalServerError.ToString(),
                    Messages = "Lỗi xử lý dữ liệu, vui lòng thử lại"
                };
            }
        }
    }
}
