using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using DGDiemRenLuyen.Services;
using System;

namespace Business.APIBusinessServices.CountryServices
{
    public class ChildCriterionDeleteService : BaseService<Guid?, string>
    {
        private readonly IChildCriteriaRepository _childCriteriaRepository;


        public ChildCriterionDeleteService(
           IChildCriteriaRepository childCriteriaRepository,
           IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor
        )
        {
            _childCriteriaRepository = childCriteriaRepository;
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

            ChildCriterion childCriterion = _childCriteriaRepository.GetById(_dataRequest);

            if ((childCriterion == null) || (childCriterion.Id == null))
            {
                throw new BaseException { Messages = "Id không tồn tại đúng!" };
            }
        }

        public override void P3AccessDatabase()
        {
            _childCriteriaRepository.Delete<Guid?>(_dataRequest);
            _childCriteriaRepository.Save();

        }

        public override void P4GenerateResponseData()
        {
            _dataResponse = "Xóa thành công!";

        }
    }
}

