using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.ChildCriterionService
{
    public class ChildCriterionGetDetailService : BaseService<Guid?, ChildCriterionResponse>
    {
        private readonly IChildCriteriaRepository _childCriteriaRepository;
        private ChildCriterion? childCriterion;

        public ChildCriterionGetDetailService(
            IChildCriteriaRepository childCriteriaRepository,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _childCriteriaRepository = childCriteriaRepository;
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
            childCriterion = _childCriteriaRepository.GetById(_dataRequest.Value);
            if (childCriterion == null)
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
            if(childCriterion != null)
            {
                _dataResponse = new ChildCriterionResponse
                {
                    Id = childCriterion.Id,
                    ParentCriteriaId = childCriterion.ParentCriteriaId,
                    CriteriaName = childCriterion.CriteriaName,
                    MaxScore = childCriterion.MaxScore,
                    OrderIndex = childCriterion.OrderIndex,
                    IsActive = childCriterion.IsActive,
                    Note = childCriterion.Note,
                    CriteriaType = childCriterion.CriteriaType,
                    CreatedAt = childCriterion.CreatedAt,
                    UpdatedAt = childCriterion.UpdatedAt,
                };
            }
        }
    }
}
