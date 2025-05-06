using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Extentions;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.ParentCriterionService
{
    public class ParentCriterionUpdateService : BaseService<ParentCriterionRequest, ParentCriterionResponse>
    {
        private readonly IParentCriteriaRepository _parentCriteriaRepository;
        private readonly IChildCriteriaRepository _childCriteriaRepository;
        private ParentCriterion? updateParentCriterion;
        private bool checkCriteriaNameUpdate = false;
        private bool checkOrderIndexUpdate = false;
        private bool checkIsActiveUpdate = false;

        public ParentCriterionUpdateService(
            IParentCriteriaRepository parentCriteriaRepository,
            IChildCriteriaRepository childCriteriaRepository,
            IHttpContextAccessor httpContextAccessor,
             string successMessageDefault = ValidationKeyWords.UPDATE) : base(httpContextAccessor, successMessageDefault)
        {
            _parentCriteriaRepository = parentCriteriaRepository;
            _childCriteriaRepository = childCriteriaRepository;
        }

        public override void P1GenerateObjects()
        {
            updateParentCriterion = _parentCriteriaRepository.GetById<Guid?>(_dataRequest.Id);
            if(updateParentCriterion == null)
            {
                throw new BaseException { Messages = "Tiêu chí không tồn tại." };
            }

            checkCriteriaNameUpdate = !String.IsNullOrEmpty(_dataRequest.CriteriaName) && _dataRequest.CriteriaName != updateParentCriterion.CriteriaName;
            checkOrderIndexUpdate = _dataRequest.OrderIndex != null && _dataRequest.OrderIndex != updateParentCriterion.OrderIndex;
            checkIsActiveUpdate = _dataRequest.IsActive != null && _dataRequest.IsActive != updateParentCriterion.IsActive;


            updateParentCriterion.CriteriaName = _dataRequest.CriteriaName ?? updateParentCriterion.CriteriaName;
            updateParentCriterion.MaxScore = _dataRequest.MaxScore ?? updateParentCriterion.MaxScore;
            updateParentCriterion.OrderIndex = _dataRequest.OrderIndex ?? updateParentCriterion.OrderIndex;
            updateParentCriterion.IsActive = _dataRequest.IsActive ?? updateParentCriterion.IsActive;
            updateParentCriterion.Note = _dataRequest.Note ?? updateParentCriterion.Note;


        }

        public override void P2PostValidation()
        {
            if (checkCriteriaNameUpdate)
            {
                if (_parentCriteriaRepository.ExistsBy(pc => pc.CriteriaName == _dataRequest.CriteriaName))
                {
                    throw new BaseException { Messages = "Tên tiêu chí tồn tại." };
                }
            }

            /*if (checkOrderIndexUpdate)
            {
                if (_parentCriteriaRepository.ExistsBy(pc => pc.OrderIndex == _dataRequest.OrderIndex))
                {
                    throw new BaseException { Messages = "Thứ tự xuất hiện đã tồn tại." };
                }
            }*/
            
        }

        public override void P3AccessDatabase()
        {
            if(updateParentCriterion != null)
            {
                _parentCriteriaRepository.Update(updateParentCriterion);
                var result = _parentCriteriaRepository.Save();

                if(result > 0 && checkIsActiveUpdate)
                {
                    _childCriteriaRepository.UpdateIsActiveByParentCriteriaId(_dataRequest.IsActive, _dataRequest.Id);
                }
            }
        }

        public override void P4GenerateResponseData()
        {
            if(updateParentCriterion != null)
            {
                _dataResponse = new ParentCriterionResponse
                {
                    Id = updateParentCriterion.Id,
                    CriteriaName = updateParentCriterion.CriteriaName,
                    MaxScore = updateParentCriterion.MaxScore,
                    OrderIndex = updateParentCriterion.OrderIndex,
                    IsActive = updateParentCriterion.IsActive,
                    Note = updateParentCriterion.Note,
                    CreatedAt = updateParentCriterion.CreatedAt,
                    UpdatedAt = updateParentCriterion.UpdatedAt,
                };
            }
        }
    }
}
