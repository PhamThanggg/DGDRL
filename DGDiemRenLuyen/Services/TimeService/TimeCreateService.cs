using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Extentions;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.TimeService
{
    public class TimeCreateService : BaseService<TimeRequest, TimeResponse>
    {
        private readonly ITimeRepository _timeRepository;
        private Time? newTime;
        private Guid newID = Guid.NewGuid();

        public TimeCreateService(
            ITimeRepository timeRepository,
            IHttpContextAccessor httpContextAccessor,
            string successMessageDefault = ValidationKeyWords.CREATE) : base(httpContextAccessor, successMessageDefault)
        {
            _timeRepository = timeRepository;
        }

        public override void P1GenerateObjects()
        {
            newTime = new Time
            {
                Id = newID,
                TermID = _dataRequest.TermID,
                StartYear = _dataRequest.StartYear,
                EndYear = _dataRequest.StartYear + 1,
                StartDate = _dataRequest.StartDate,
                EndDate = _dataRequest.EndDate,
                CreatedBy = UserID
             };
        }

        public override void P2PostValidation()
        {
            if (string.IsNullOrEmpty(UserID))
            {
                throw new BaseException { Messages = "Bạn không có quyền truy cập" };
            }

            DateTime now = DateTime.UtcNow.AddHours(7).Date;
            if (_dataRequest.StartDate?.Date < now)
            {
                throw new BaseException { Messages = "Ngày bắt đầu không được là quá khứ." };
            }

            if (_dataRequest.EndDate <= _dataRequest.StartDate)
            {
                throw new BaseException { Messages = "Ngày kết thúc phải lớn hơn ngày bắt đầu." };
            }


            bool semesterExists = _timeRepository.ExistsBy(pc =>
                pc.StartYear == _dataRequest.StartYear &&
                pc.TermID == _dataRequest.TermID
            );

            if (semesterExists)
            {
                throw new BaseException { Messages = "Kỳ học này đã tồn tại thời gian xét điểm rèn luyện!" };
            }
        }

        public override void P3AccessDatabase()
        {
            _timeRepository.Add(newTime);
            _timeRepository.Save();
        }

        public override void P4GenerateResponseData()
        {
            if(newTime != null)
            {
                _dataResponse = new TimeResponse
                {
                    Id = newTime.Id,
                    TermID = newTime.TermID,
                    StartYear = newTime.StartYear,
                    EndYear = newTime.StartYear + 1,
                    StartDate = newTime.StartDate,
                    EndDate = newTime.EndDate,
                    CreatedBy = newTime.CreatedBy,
                    CreatedAt = newTime.CreatedAt,
                    UpdatedAt = newTime.UpdatedAt,
                };
            }
        }
    }
}
