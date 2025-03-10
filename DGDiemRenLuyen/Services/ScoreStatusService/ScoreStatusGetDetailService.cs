using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.ScoreStatusService
{
    public class ScoreStatusGetDetailService : BaseService<ScoreStatusRequest, ScoreStatusResponse>
    {
        private readonly IScoreStatusRepository _scoreStatusRepository;
        private readonly ITimeRepository _timeRepository;
        private readonly ScoreStatusCreateService _scoreStatusCreateService;
        private ScoreStatus? _scoreStatus;

        public ScoreStatusGetDetailService(
            IScoreStatusRepository scoreStatusRepository,
            ITimeRepository timeRepository,
            ScoreStatusCreateService scoreStatusCreateService,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _scoreStatusRepository = scoreStatusRepository;
            _timeRepository = timeRepository;
            _scoreStatusCreateService = scoreStatusCreateService;
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
            var timeData = _timeRepository.GetById(_dataRequest.TimeId);
            if (timeData == null)
            {
                throw new BaseException { Messages = "Thời gian không tồn tại!" }; ;
            }

            _scoreStatus = _scoreStatusRepository.FindByStudentIdAndTimeId(UserID, _dataRequest.TimeId);

            if (_scoreStatus == null)
            {
                ScoreStatusResponse scoreStatusResponse 
                    = _scoreStatusCreateService.create(UserID);

                _dataResponse = scoreStatusResponse;
            }


        }

        public override void P4GenerateResponseData()
        {
            if(_scoreStatus != null)
            {
                _dataResponse = new ScoreStatusResponse
                {
                    Id = _scoreStatus.Id,
                    StudentId = _scoreStatus.StudentId,
                    TimeId = _scoreStatus.TimeId,
                    Status = _scoreStatus.Status,
                    CreatedAt = _scoreStatus.CreatedAt,
                    UpdatedAt = _scoreStatus.UpdatedAt,
                };
            }
        }
    }
}
