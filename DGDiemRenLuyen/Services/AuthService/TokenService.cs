using DGDiemRenLuyen.DTOs.Responses;

namespace DGDiemRenLuyen.Services.AuthService
{
    public class TokenService : BaseService<string, TokenResponse>
    {
        private readonly AuthService _authService;


        public TokenService(
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
            _dataResponse = _authService.GenerateJwtToken(_dataRequest);
        }
    }
}
