using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DGDiemRenLuyen.Services.ScoreStatusService
{
    public class ScoreStatusUpdateService : BaseService<ScoreStatusRequest, ScoreStatusResponse>
    {
        private readonly IScoreStatusRepository _scoreStatusRepository;
        private ScoreStatus? updateScoreStatus;

        public ScoreStatusUpdateService(
            IScoreStatusRepository scoreStatusRepository,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _scoreStatusRepository = scoreStatusRepository;
        }

        public override void P1GenerateObjects()
        {
            updateScoreStatus = _scoreStatusRepository.GetById<Guid?>(_dataRequest.Id);
            if(updateScoreStatus == null)
            {
                throw new BaseException { Messages = "Điểm rèn luyên không tồn tại." };
            }

            // chưa phân quyền {trạng thái duyệt}
            updateScoreStatus.Status = _dataRequest.status ?? updateScoreStatus.Status;

        }

        public override void P2PostValidation()
        {
            /*if (string.IsNullOrEmpty(UserID))
            {
                throw new BaseException { Messages = "Bạn không có quyền truy cập" };
            }*/

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
                    Id = updateScoreStatus.Id,
                    StudentId = updateScoreStatus.StudentId,
                    TimeId = updateScoreStatus.TimeId,
                    Status = updateScoreStatus.Status,
                    CreatedAt = updateScoreStatus.CreatedAt,
                    UpdatedAt = updateScoreStatus.UpdatedAt,
                };
            }
        }
    }
}
