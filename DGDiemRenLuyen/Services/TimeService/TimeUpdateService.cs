using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DGDiemRenLuyen.Services.TimeService
{
    public class TimeUpdateService : BaseService<TimeRequest, TimeResponse>
    {
        private readonly ITimeRepository _timeRepository;
        private Time? updateTime;
        private bool checkSemesterUpdate = false;
        private bool checkStartYearUpdate = false;
        private bool checkStartDateUpdate = false;
        private bool checkEndDateUpdate = false;

        public TimeUpdateService(
            ITimeRepository timeRepository,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _timeRepository = timeRepository;
        }

        public override void P1GenerateObjects()
        {
            updateTime = _timeRepository.GetById<Guid?>(_dataRequest.Id);
            if(updateTime == null)
            {
                throw new BaseException { Messages = "Lịch đánh giá điểm rèn luyện không tồn tại." };
            }

            checkSemesterUpdate = _dataRequest.Semester != null && _dataRequest.Semester != updateTime.Semester;
            checkStartYearUpdate = _dataRequest.StartYear != null && _dataRequest.StartYear != updateTime.StartYear;
            checkStartDateUpdate = _dataRequest.StartDate.HasValue && _dataRequest.StartDate != updateTime.StartDate;
            checkEndDateUpdate = _dataRequest.EndDate.HasValue && _dataRequest.EndDate != updateTime.EndDate;

            updateTime.StartYear = _dataRequest.StartYear ?? updateTime.StartYear;
            updateTime.EndYear = _dataRequest.StartYear + 1 ?? updateTime.EndYear;
            updateTime.StartDate = _dataRequest.StartDate ?? updateTime.StartDate;
            updateTime.EndDate = _dataRequest.EndDate ?? updateTime.EndDate;
            updateTime.CreatedBy = UserID ?? updateTime.CreatedBy;

        }

        public override void P2PostValidation()
        {
            if (string.IsNullOrEmpty(UserID))
            {
                throw new BaseException { Messages = "Bạn không có quyền truy cập" };
            }

            DateTime now = DateTime.UtcNow.AddHours(7).Date;

            if (checkStartDateUpdate)
            {
                if (updateTime.StartDate?.Date < now)
                {
                    throw new BaseException { Messages = "Ngày bắt đầu không được là quá khứ." };
                }
            }

            if (checkStartDateUpdate || checkEndDateUpdate)
            {
                if (updateTime.EndDate <= updateTime.StartDate)
                {
                    throw new BaseException { Messages = "Ngày kết thúc phải lớn hơn ngày bắt đầu." };
                }
            }

            if (checkSemesterUpdate || checkStartYearUpdate)
            {
                bool semesterExists = _timeRepository.ExistsBy(pc =>
                    pc.StartYear == _dataRequest.StartYear &&
                    pc.Semester == _dataRequest.Semester
                );

                if (semesterExists)
                {
                    throw new BaseException { Messages = "Kỳ học này đã tồn tại thời gian xét điểm rèn luyện!" };
                }
            }
        }

        public override void P3AccessDatabase()
        {
            if(updateTime != null)
            {
                _timeRepository.Update(updateTime);
                _timeRepository.Save();
            }
        }

        public override void P4GenerateResponseData()
        {
            if(updateTime != null)
            {
                _dataResponse = new TimeResponse
                {
                    Id = updateTime.Id,
                    Semester = updateTime.Semester,
                    StartYear = updateTime.StartYear,
                    EndYear = updateTime.StartYear + 1,
                    StartDate = updateTime.StartDate,
                    EndDate = updateTime.EndDate,
                    CreatedBy = updateTime.CreatedBy,
                    CreatedAt = updateTime.CreatedAt,
                    UpdatedAt = updateTime.UpdatedAt,
                };
            }
        }
    }
}
