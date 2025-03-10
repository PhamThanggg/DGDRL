using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.ParentCriterionService
{
    public class ParentCriterionGetListDetailService : BaseService<ParentCriterionGetListDetailRequest, Task<List<ParentCriterion>>>
    {
        private readonly IParentCriteriaRepository _parentCriteriaRepository;
        private Task<List<ParentCriterion>>? parentCriterions;

        public ParentCriterionGetListDetailService(
            IParentCriteriaRepository parentCriteriaRepository,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _parentCriteriaRepository = parentCriteriaRepository;
        }

        public override void P1GenerateObjects()
        {
        }

        public override void P2PostValidation()
        {
            
        }

        public override void P3AccessDatabase()
        {
            parentCriterions = _parentCriteriaRepository.GetParentCriteriaByStudentIdAsync(_dataRequest.UserId, _dataRequest.TimeId);
            if (parentCriterions == null)
            {
                throw new BaseException
                {
                    StatusCode = StatusCodes.Status404NotFound.ToString(),
                    Messages = "Form đánh giá này không tồn tại!"
                };
            }
        }

        public override void P4GenerateResponseData()
        {
            if(parentCriterions != null)
            {
                _dataResponse = parentCriterions;
            }
        }
    }
}
