using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Extentions;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using DGDiemRenLuyen.Services;
using System;

namespace DGDiemRenLuyen.Services.ParentCriterionService
{
    public class ParentCriterionDeleteService : BaseService<Guid?, string>
    {
        private readonly IParentCriteriaRepository _parentCriteriaRepository;
        private readonly IChildCriteriaRepository _childCriteriaRepository;


        public ParentCriterionDeleteService(
           IParentCriteriaRepository parentCriteriaRepository,
           IChildCriteriaRepository childCriteriaRepository,
           IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor
        )
        {
            _parentCriteriaRepository = parentCriteriaRepository;
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

            ParentCriterion parentCriterion = _parentCriteriaRepository.GetById(_dataRequest);

            if ((parentCriterion == null) || (parentCriterion.Id == null))
            {
                throw new BaseException { Messages = "Id không tồn tại đúng!" };
            }

            ChildCriterion childCriterion = _childCriteriaRepository
                .GetBy(s => s.ParentCriteriaId == parentCriterion.Id)
                .FirstOrDefault();

            if (childCriterion != null)
            {
                throw new BaseException { Messages = ValidationKeyWords.VALID_RFR };
            }

        }

        public override void P3AccessDatabase()
        {
            _parentCriteriaRepository.Delete<Guid?>(_dataRequest);
            _parentCriteriaRepository.Save();

        }

        public override void P4GenerateResponseData()
        {
            _dataResponse = ValidationKeyWords.DELETE.ToString();

        }
    }
}

