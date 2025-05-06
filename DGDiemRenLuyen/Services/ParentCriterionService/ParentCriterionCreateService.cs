using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Extentions;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.ParentCriterionService
{
    public class ParentCriterionCreateService : BaseService<ParentCriterionRequest, ParentCriterionResponse>
    {
        private readonly IParentCriteriaRepository _parentCriteriaRepository;
        private ParentCriterion? newParentCriterion;
        private Guid newID = Guid.NewGuid();

        public ParentCriterionCreateService(
            IParentCriteriaRepository parentCriteriaRepository,
            IHttpContextAccessor httpContextAccessor,
            string successMessageDefault = ValidationKeyWords.CREATE) : base (httpContextAccessor, successMessageDefault)
        {
            _parentCriteriaRepository = parentCriteriaRepository;
        }

        public override void P1GenerateObjects()
        {
            newParentCriterion = new ParentCriterion
            {
                CriteriaName = _dataRequest.CriteriaName,
                MaxScore = _dataRequest.MaxScore,
                OrderIndex = _dataRequest.OrderIndex,
                IsActive = _dataRequest.IsActive,
                Note = _dataRequest.Note,
            };
        }

        public override void P2PostValidation()
        {
            if(_parentCriteriaRepository.ExistsBy(pc => pc.CriteriaName == _dataRequest.CriteriaName))
            {
                throw new BaseException { Messages = "Tên tiêu chí tồn tại." };
            }
            /*if(_parentCriteriaRepository.ExistsBy(pc => pc.OrderIndex == _dataRequest.OrderIndex))
            {
                throw new BaseException { Messages = "Thứ tự xuất hiện đã tồn tại." };
            }*/
        }

        public override void P3AccessDatabase()
        {
            _parentCriteriaRepository.Add(newParentCriterion);
            _parentCriteriaRepository.Save();
        }

        public override void P4GenerateResponseData()
        {
            if(newParentCriterion != null)
            {
                _dataResponse = new ParentCriterionResponse
                {
                    Id = newID,
                    CriteriaName = newParentCriterion.CriteriaName,
                    MaxScore = newParentCriterion.MaxScore,
                    OrderIndex = newParentCriterion.OrderIndex,
                    IsActive = newParentCriterion.IsActive,
                    Note = newParentCriterion.Note,
                    CreatedAt = newParentCriterion.CreatedAt,
                    UpdatedAt = newParentCriterion.UpdatedAt,
                };
            }
        }
    }
}
