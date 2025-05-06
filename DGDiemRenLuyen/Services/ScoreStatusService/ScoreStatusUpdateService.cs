using DGDiemRenLuyen.Common;
using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Extentions;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.ScoreStatusService
{
    public class ScoreStatusUpdateService : BaseService<ScoreStatusRequest, ScoreStatusResponse>
    {
        private readonly IScoreStatusRepository _scoreStatusRepository;
        private ScoreStatus? updateScoreStatus;

        public ScoreStatusUpdateService(
            IScoreStatusRepository scoreStatusRepository,
            IHttpContextAccessor httpContextAccessor,
            string successMessageDefault = ValidationKeyWords.UPDATE) : base(httpContextAccessor, successMessageDefault)
        {
            _scoreStatusRepository = scoreStatusRepository;
        }

        public override void P1GenerateObjects()
        {
            updateScoreStatus = _scoreStatusRepository.GetById<Guid?>(_dataRequest.Id);
            if(updateScoreStatus == null)
            {
                throw new BaseException { Messages = "Điểm rèn luyện không tồn tại." };
            }

            updateScoreStatus.Status = _dataRequest.status ?? updateScoreStatus.Status;

            updateScoreStatus.SeductedPoint = _dataRequest.SeductedPoint ?? updateScoreStatus.SeductedPoint;
            updateScoreStatus.PlusPoint = _dataRequest.PlusPoint ?? updateScoreStatus.PlusPoint;
            updateScoreStatus.Note = _dataRequest.Note ?? updateScoreStatus.Note;
        }

        public override void P2PostValidation()
        {
            // begin phan quyen
            // Trang thai
            if (_dataRequest.status > ScoreStatusConstants.SV && Role == RoleConstants.SV)
            {
                throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
            }
            else if (_dataRequest.status > ScoreStatusConstants.GV && Role == RoleConstants.GV)
            {
                throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
            }
            else if (_dataRequest.status > ScoreStatusConstants.CBL && Role == RoleConstants.CBL)
            {
                throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
            }
            else if (_dataRequest.status > ScoreStatusConstants.TK && Role == RoleConstants.TK)
            {
                throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
            }

            if (Role == RoleConstants.SV && 
                    (_dataRequest.SeductedPoint != null 
                    || _dataRequest.PlusPoint != null 
                    || !string.IsNullOrWhiteSpace(_dataRequest.Note))
               )
            {
                throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
            }
            // end
        }

        public override void P3AccessDatabase()
        {
            if(updateScoreStatus != null)
            {
                _scoreStatusRepository.Update(updateScoreStatus);
                _scoreStatusRepository.Save();
            }
        }

        public override void P4GenerateResponseData()
        {
            if(updateScoreStatus != null)
            {
                _dataResponse = new ScoreStatusResponse
                {
                    ScoreStatusId = updateScoreStatus.Id,
                    StudentID = updateScoreStatus.StudentId,
                    TimeId = updateScoreStatus.TimeId,
                    Status = updateScoreStatus.Status,
                    SeductedPoint = updateScoreStatus.SeductedPoint,
                    PlusPoint = updateScoreStatus.PlusPoint,
                    Note = updateScoreStatus.Note,
                    CreatedAt = updateScoreStatus.CreatedAt,
                    UpdatedAt = updateScoreStatus.UpdatedAt,
                };
            }
        }
    }
}
