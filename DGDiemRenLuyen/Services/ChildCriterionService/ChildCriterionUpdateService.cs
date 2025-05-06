using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Extentions;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.ChildCriterionService
{
    public class ChildCriterionUpdateService : BaseService<ChildCriterionRequest, ChildCriterionResponse>
    {
        private readonly IChildCriteriaRepository _childCriteriaRepository;
        private ChildCriterion? updateChildCriterion;
        private bool checkCriteriaNameUpdate = false;
        private bool checkOrderIndexUpdate = false;

        public ChildCriterionUpdateService(
            IChildCriteriaRepository childCriteriaRepository,
            IHttpContextAccessor httpContextAccessor,
            string successMessageDefault = ValidationKeyWords.UPDATE) : base(httpContextAccessor, successMessageDefault)
        {
            _childCriteriaRepository = childCriteriaRepository;
        }

        public override void P1GenerateObjects()
        {
            updateChildCriterion = _childCriteriaRepository.GetById<Guid?>(_dataRequest.Id);
            if(updateChildCriterion == null)
            {
                throw new BaseException { Messages = "Tiêu chí không tồn tại." };
            }

            checkCriteriaNameUpdate = !String.IsNullOrEmpty(_dataRequest.CriteriaName) && _dataRequest.CriteriaName != updateChildCriterion.CriteriaName;
            checkOrderIndexUpdate = _dataRequest.OrderIndex != null && _dataRequest.OrderIndex != updateChildCriterion.OrderIndex;


            updateChildCriterion.CriteriaName = _dataRequest.CriteriaName ?? updateChildCriterion.CriteriaName;
            updateChildCriterion.MaxScore = _dataRequest.MaxScore ?? updateChildCriterion.MaxScore;
            updateChildCriterion.OrderIndex = _dataRequest.OrderIndex ?? updateChildCriterion.OrderIndex;
            updateChildCriterion.IsActive = _dataRequest.IsActive ?? updateChildCriterion.IsActive;
            updateChildCriterion.Note = _dataRequest.Note ?? updateChildCriterion.Note;
            updateChildCriterion.CriteriaType = _dataRequest.CriteriaType ?? updateChildCriterion.CriteriaType;
        }

        public override void P2PostValidation()
        {
            if (checkCriteriaNameUpdate)
            {
                if (_childCriteriaRepository.ExistsByNameAndParentCriteriaId
                    (_dataRequest.CriteriaName, updateChildCriterion.ParentCriteriaId))
                {
                    throw new BaseException { Messages = "Tên tiêu chí tồn tại." };
                }
                
            }

           /* if (checkOrderIndexUpdate)
            {
                if (_childCriteriaRepository.ExistsByOrderIndexAndParentCriteriaId
                    (_dataRequest.OrderIndex ?? 1, updateChildCriterion.ParentCriteriaId))
                {
                    throw new BaseException { Messages = "Thứ tự xuất hiện đã tồn tại." };
                }
            }*/
            
        }

        public override void P3AccessDatabase()
        {
            if(updateChildCriterion != null)
            {
                _childCriteriaRepository.Update(updateChildCriterion);
                _childCriteriaRepository.Save();
            }
        }

        public override void P4GenerateResponseData()
        {
            if(updateChildCriterion != null)
            {
                _dataResponse = new ChildCriterionResponse
                {
                    Id = updateChildCriterion.Id,
                    ParentCriteriaId = updateChildCriterion.ParentCriteriaId,
                    CriteriaName = updateChildCriterion.CriteriaName,
                    MaxScore = updateChildCriterion.MaxScore,
                    OrderIndex = updateChildCriterion.OrderIndex,
                    IsActive = updateChildCriterion.IsActive,
                    CriteriaType = updateChildCriterion.CriteriaType,
                    Note = updateChildCriterion.Note,
                    CreatedAt = updateChildCriterion.CreatedAt,
                    UpdatedAt = updateChildCriterion.UpdatedAt,
                };
            }
        }
    }
}
