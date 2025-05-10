using DGDiemRenLuyen.DTOs.Requests;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.AuthService
{
    public class RefreshTokenService : BaseService<string, TokenResponse>
    {
        private readonly AuthService _authService;


        public RefreshTokenService(
            IHttpContextAccessor httpContextAccessor,
            AuthService authService,
            string successMessageDefault = "") : base(httpContextAccessor, successMessageDefault)
        {
            _authService = authService;
        }

        public override void P1GenerateObjects()
        {
        }

        public override void P2PostValidation()
        {
        }

        public override void P3AccessDatabase()
        {
        }

        public override void P4GenerateResponseData()
        {
            _dataResponse = _authService.RefreshJwtToken(_dataRequest);
        }
    }
}
