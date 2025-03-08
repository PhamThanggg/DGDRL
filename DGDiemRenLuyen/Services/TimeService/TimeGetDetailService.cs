using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using System;

namespace DGDiemRenLuyen.Services.TimeService
{
    public class TimeGetDetailService : BaseService<Guid?, TimeResponse>
    {
        private readonly ITimeRepository _TimeRepository;
        private Time? _time;

        public TimeGetDetailService(ITimeRepository TimeRepository,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _TimeRepository = TimeRepository;
        }

        public override void P1GenerateObjects()
        {
            if (!_dataRequest.HasValue)
            {
                throw new BaseException { Messages = "Giá trị truyền vào không hợp lệ." };
            }
        }

        public override void P2PostValidation()
        {
            
        }

        public override void P3AccessDatabase()
        {
            _time = _TimeRepository.GetById(_dataRequest.Value);
            if (_time == null)
            {
                throw new BaseException
                {
                    StatusCode = StatusCodes.Status404NotFound.ToString(),
                    Messages = "Id không tồn tại!"
                };
            }
        }

        public override void P4GenerateResponseData()
        {
            if(_time != null)
            {
                _dataResponse = new TimeResponse
                {
                    Id = _time.Id,
                    Semester = _time.Semester,
                    StartYear = _time.StartYear,
                    EndYear = _time.StartYear + 1,
                    StartDate = _time.StartDate,
                    EndDate = _time.EndDate,
                    CreatedBy = _time.CreatedBy,
                    CreatedAt = _time.CreatedAt,
                    UpdatedAt = _time.UpdatedAt,
                };
            }
        }
    }
}
