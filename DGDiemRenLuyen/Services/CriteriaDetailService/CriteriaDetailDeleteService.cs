using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.CriteriaDetailService
{
    public class CriteriaDetailDeleteService : BaseService<Guid?, string>
    {
        private readonly ICriteriaDetailRepository _criteriaDetailRepository;


        public CriteriaDetailDeleteService(
           ICriteriaDetailRepository criteriaDetailRepository,
           IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _criteriaDetailRepository = criteriaDetailRepository;
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

            CriteriaDetail parentCriterion = _criteriaDetailRepository.GetById(_dataRequest);

            if ((parentCriterion == null) || (parentCriterion.Id == null))
            {
                throw new BaseException { Messages = "Id không tồn tại đúng!" };
            }
        }

        public override void P3AccessDatabase()
        {
            _criteriaDetailRepository.Delete<Guid?>(_dataRequest);
            _criteriaDetailRepository.Save();

        }

        public override void P4GenerateResponseData()
        {
            _dataResponse = "Xóa thành công!";

        }
    }
}

