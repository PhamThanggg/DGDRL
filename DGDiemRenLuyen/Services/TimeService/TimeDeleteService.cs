using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.TimeService
{
    public class TimeDeleteService : BaseService<Guid?, string>
    {
        private readonly ITimeRepository _timeRepository;


        public TimeDeleteService(
           ITimeRepository timeRepository,
           IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _timeRepository = timeRepository;
        }

        public override void P1GenerateObjects()
        {
            
        }

        public override void P2PostValidation()
        {
            if (_dataRequest == null)
            {
                throw new BaseException { Messages = "Id không đúng!" };
            }

            Time parentCriterion = _timeRepository.GetById(_dataRequest);

            if ((parentCriterion == null) || (parentCriterion.Id == null))
            {
                throw new BaseException { Messages = "Id không tồn tại đúng!" };
            }
        }

        public override void P3AccessDatabase()
        {
            _timeRepository.Delete<Guid?>(_dataRequest);
            _timeRepository.Save();

        }

        public override void P4GenerateResponseData()
        {
            _dataResponse = "Xóa thành công!";

        }
    }
}

