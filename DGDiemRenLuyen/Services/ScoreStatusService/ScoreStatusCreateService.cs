using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.ScoreStatusService
{
    public class ScoreStatusCreateService
    {
        private readonly IScoreStatusRepository _scoreStatusRepository;
        private readonly ITimeRepository _timeRepository;
        private ScoreStatus? newScoreStatus;
        private Guid newID = Guid.NewGuid();
        private Guid timeId;

        public ScoreStatusCreateService(
            IScoreStatusRepository scoreStatusRepository,
            ITimeRepository timeRepository) 
        {
            _scoreStatusRepository = scoreStatusRepository;
            _timeRepository = timeRepository;
        }

        public ScoreStatusResponse create(string UserID)
        {
            // kiểm tra thời gian xét có tồn tại k?
            Time? timeData = _timeRepository.GetCurrentTimeRecords();

            if(timeData == null)
            {
                throw new BaseException { Messages = "Đã hết thời gian xét điểm rèm luyên!" };
            }

            timeId = timeData.Id;
            newScoreStatus = new ScoreStatus
            {
                Id = newID,
                StudentId = UserID,
                TimeId = timeData.Id,
                Status = 0
             };

            if (_scoreStatusRepository.ExistsByStudentIdAndTimeId(UserID, timeId))
            {
                return null;
            }

            // begin thên vào db
            _scoreStatusRepository.Add(newScoreStatus);
            _scoreStatusRepository.Save();
            // end thêm vào db

            if (newScoreStatus != null)
            {
                return new ScoreStatusResponse
                {
                    Id = newScoreStatus.Id,
                    StudentId = newScoreStatus.StudentId,
                    TimeId = newScoreStatus.TimeId,
                    Status = newScoreStatus.Status,
                    CreatedAt = newScoreStatus.CreatedAt,
                    UpdatedAt = newScoreStatus.UpdatedAt,
                };

            }

            return null;
        }
    }
}
