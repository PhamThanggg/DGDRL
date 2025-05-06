using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Extentions;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using DGDiemRenLuyen.Services;
using System;

namespace Business.APIBusinessServices.CountryServices
{
    public class ChildCriterionDeleteService : BaseService<Guid?, string>
    {
        private readonly IChildCriteriaRepository _childCriteriaRepository;
        private readonly ICriteriaDetailRepository _criteriaDetailRepository;


        public ChildCriterionDeleteService(
           IChildCriteriaRepository childCriteriaRepository,
           ICriteriaDetailRepository criteriaDetailRepository,
           IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor
        )
        {
            _childCriteriaRepository = childCriteriaRepository;
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

            ChildCriterion childCriterion = _childCriteriaRepository.GetById(_dataRequest);

            if ((childCriterion == null) || (childCriterion.Id == null))
            {
                throw new BaseException { Messages = "Id không tồn tại đúng!" };
            }

            CriteriaDetail criteriaDetail = _criteriaDetailRepository
               .GetBy(s => s.ChildCriteriaId == childCriterion.Id)
               .FirstOrDefault();

            if (criteriaDetail != null)
            {
                throw new BaseException { Messages = ValidationKeyWords.VALID_RFR };
            }
        }

        public override void P3AccessDatabase()
        {
            _childCriteriaRepository.Delete<Guid?>(_dataRequest);
            _childCriteriaRepository.Save();

        }

        public override void P4GenerateResponseData()
        {
            _dataResponse = ValidationKeyWords.DELETE.ToString();

        }
    }
}

