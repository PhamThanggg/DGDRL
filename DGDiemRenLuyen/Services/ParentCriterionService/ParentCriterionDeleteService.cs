using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using DGDiemRenLuyen.Services;
using System;

namespace DGDiemRenLuyen.Services.ParentCriterionService
{
    public class ParentCriterionDeleteService : BaseService<Guid?, string>
    {
        private readonly IParentCriteriaRepository _parentCriteriaRepository;


        public ParentCriterionDeleteService(
           IParentCriteriaRepository parentCriteriaRepository,
           IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor
        )
        {
            _parentCriteriaRepository = parentCriteriaRepository;
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

            ParentCriterion parentCriterion = _parentCriteriaRepository.GetById(_dataRequest);

            if ((parentCriterion == null) || (parentCriterion.Id == null))
            {
                throw new BaseException { Messages = "Id không tồn tại đúng!" };
            }
        }

        public override void P3AccessDatabase()
        {
            _parentCriteriaRepository.Delete<Guid?>(_dataRequest);
            _parentCriteriaRepository.Save();

        }

        public override void P4GenerateResponseData()
        {
            _dataResponse = "Xóa thành công!";

        }
    }
}

