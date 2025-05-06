using DGDiemRenLuyen.Common;
using DGDiemRenLuyen.DTOs.HnueApiResponse;
using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.DTOs.Responses.Students;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.ScoreStatusService
{
    public class ScoreStatusGetDetailService : BaseService<ScoreStatusRequest, StudentResponse>
    {
        private readonly IScoreStatusRepository _scoreStatusRepository;
        private readonly ITimeRepository _timeRepository;
        private readonly ScoreStatusCreateService _scoreStatusCreateService;
        private readonly ApiClientService _apiClientService;
        private ScoreStatus? _scoreStatus;

        public ScoreStatusGetDetailService(
            IScoreStatusRepository scoreStatusRepository,
            ITimeRepository timeRepository,
            ScoreStatusCreateService scoreStatusCreateService,
            ApiClientService apiClientService,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _scoreStatusRepository = scoreStatusRepository;
            _timeRepository = timeRepository;
            _scoreStatusCreateService = scoreStatusCreateService;
            _apiClientService = apiClientService;
        }

        public override void P1GenerateObjects()
        {
            
        }

        public override void P2PostValidation()
        {
            if (string.IsNullOrEmpty(UserID))
            {
                throw new BaseException { Messages = "Bạn không có quyền truy cập" };
            }
        }

        public override void P3AccessDatabase()
        {
            var timeData = _timeRepository.GetById(_dataRequest.TimeId)
                ?? throw new BaseException { Messages = "Thời gian không tồn tại!" };

            _scoreStatus ??= _scoreStatusRepository.FindByStudentIdAndTimeId(UserID, _dataRequest.TimeId)
                ?? _scoreStatusCreateService.create(UserID);

            string url = ApiRoute.baseUrl + ApiRoute.getDetailStudent + UserID;

            var apiTask = _apiClientService.GetDataFromApi<StudentResponse>(url);
            Task.WhenAll(apiTask);

            var apiResponse = apiTask.Result; 

            if (apiResponse?.Data != null)
            {
                _dataResponse = apiResponse.Data;
            }
        }

        public override void P4GenerateResponseData()
        {
            if(_scoreStatus != null)
            {
                _dataResponse.ScoreStatusId = _scoreStatus.Id;
                _dataRequest.StudentId= _scoreStatus.StudentId;
                _dataResponse.TimeId = _scoreStatus.TimeId;
                _dataResponse.Status = _scoreStatus.Status;
                _dataResponse.SeductedPoint = _scoreStatus.SeductedPoint;
                _dataResponse.PlusPoint = _scoreStatus.PlusPoint;
                _dataResponse.Note = _scoreStatus.Note;
                _dataResponse.CreatedAt = _scoreStatus?.CreatedAt;
                _dataResponse.UpdatedAt = _scoreStatus?.UpdatedAt;
            }
        }
    }
}
