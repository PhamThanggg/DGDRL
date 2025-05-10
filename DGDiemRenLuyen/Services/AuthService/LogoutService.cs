namespace DGDiemRenLuyen.Services.AuthService
{
    public class LogoutService : BaseService<string, string>
    {
        private readonly AuthService _authService;


        public LogoutService(
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
            _dataResponse = _authService.Logout(_dataRequest);
        }
    }
}
