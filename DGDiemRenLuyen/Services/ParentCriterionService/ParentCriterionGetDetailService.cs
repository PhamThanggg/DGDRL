using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.ParentCriterionService
{
    public class ParentCriterionGetDetailService : BaseService<Guid?, ParentCriterionResponse>
    {
        private readonly IParentCriteriaRepository _parentCriteriaRepository;
        private ParentCriterion? parentCriterion;

        public ParentCriterionGetDetailService(
            IParentCriteriaRepository parentCriteriaRepository,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _parentCriteriaRepository = parentCriteriaRepository;
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
            parentCriterion = _parentCriteriaRepository.GetById(_dataRequest.Value);
            if (parentCriterion == null)
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
            if(parentCriterion != null)
            {
                _dataResponse = new ParentCriterionResponse
                {
                    Id = parentCriterion.Id,
                    CriteriaName = parentCriterion.CriteriaName,
                    MaxScore = parentCriterion.MaxScore,
                    OrderIndex = parentCriterion.OrderIndex,
                    IsActive = parentCriterion.IsActive,
                    Note = parentCriterion.Note,
                    CreatedAt = parentCriterion.CreatedAt,
                    UpdatedAt = parentCriterion.UpdatedAt,
                };
            }
        }
    }
}
