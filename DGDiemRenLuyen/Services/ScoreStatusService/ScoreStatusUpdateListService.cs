using DGDiemRenLuyen.Common;
using DGDiemRenLuyen.DTOs.Requests;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Extentions;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace DGDiemRenLuyen.Services.ScoreStatusService
{
    public class ScoreStatusUpdateListService : BaseService<StatusUpdateListRequest, IActionResult>
    {
        private readonly IScoreStatusRepository _scoreStatusRepository;
        private readonly ITimeRepository _timeRepository;
        private readonly AuthService.AuthService _authService;

        private List<ScoreStatus>? _updatedIds = new();
        private List<Guid>? _notMatchedIds = new();
        private Time _time = null;

        public ScoreStatusUpdateListService(
            IScoreStatusRepository scoreStatusRepository,
            ITimeRepository timeRepository,
            AuthService.AuthService authService,
            IHttpContextAccessor httpContextAccessor,
            string successMessageDefault = ValidationKeyWords.UPDATE) : base(httpContextAccessor, successMessageDefault)
        {
            _scoreStatusRepository = scoreStatusRepository;
            _timeRepository = timeRepository;
            _authService = authService;
        }

        public override void P1GenerateObjects()
        {
            int statusDB = _dataRequest.Status > 0 ? _dataRequest.Status - 1 : 0;
            string RoleTK = Role;
            if(RoleTK == RoleConstants.CBL)
            {
                _updatedIds = _scoreStatusRepository.GetInIdAndStudentClassIdAndStatus(_dataRequest.Ids, ClassStudentID ,statusDB);
            }
            else if(RoleTK == RoleConstants.GV)
            {
                _time = _timeRepository.GetById(_dataRequest.TimeId);
                if (_time == null)
                {
                    throw new BaseException { Messages = "Thời gian đánh giá điểm không tồn tại!" };
                }
                var listClass = _authService.GetClasses(
                    UserID, _time.TermID
                    , _time.StartYear + "-" + _time.EndYear);

                _updatedIds = _scoreStatusRepository.GetInIdAndStatus(_dataRequest.Ids, statusDB, listClass);
            }
            else if (RoleTK == RoleConstants.TK)
            {
                _updatedIds = _scoreStatusRepository.GetInIdAndDepartmentIdAStdatus(_dataRequest.Ids, DepartmentID, statusDB);
            }

            foreach (var entity in _updatedIds)
            {
                entity.Status = _dataRequest.Status;
            }

            var update = _updatedIds.Select(e => e.Id).ToList();
            _notMatchedIds = _dataRequest.Ids.Except(update).ToList();
        }

        public override void P2PostValidation()
        {
        }

        public override void P3AccessDatabase()
        {
            if(_updatedIds != null)
            {
                _scoreStatusRepository.UpdateRange(_updatedIds);
                _scoreStatusRepository.Save();
            }
        }

        public override void P4GenerateResponseData()
        {
            _dataResponse = new OkObjectResult(new
            {
                Updated = _updatedIds.Count,
                UpdatedIds = _updatedIds.Select(e => e.Id),
                SkippedIds = _notMatchedIds
            });
        }
    }
}
