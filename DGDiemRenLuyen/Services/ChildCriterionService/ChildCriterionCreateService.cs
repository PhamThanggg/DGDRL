﻿using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Extentions;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.ChildCriterionService
{
    public class ChildCriterionCreateService : BaseService<ChildCriterionRequest, ChildCriterionResponse>
    {
        private readonly IChildCriteriaRepository _childCriteriaRepository;
        private readonly IParentCriteriaRepository _parentCriteriaRepository;
        private ChildCriterion? newChildCriterion;
        private Guid newID = Guid.NewGuid();

        public ChildCriterionCreateService(
            IChildCriteriaRepository childCriteriaRepository,
            IParentCriteriaRepository parentCriteriaRepository,
            IHttpContextAccessor httpContextAccessor,
            string successMessageDefault = ValidationKeyWords.CREATE) : base(httpContextAccessor, successMessageDefault)
        {
            _childCriteriaRepository = childCriteriaRepository;
            _parentCriteriaRepository = parentCriteriaRepository;
        }

        public override void P1GenerateObjects()
        {
            newChildCriterion = new ChildCriterion
            {
                CriteriaName = _dataRequest.CriteriaName,
                MaxScore = _dataRequest.MaxScore,
                OrderIndex = _dataRequest.OrderIndex,
                IsActive = _dataRequest.IsActive,
                Note = _dataRequest.Note,
                CriteriaType = _dataRequest.CriteriaType,
                ParentCriteriaId = _dataRequest.ParentCriteriaId
            };
        }

        public override void P2PostValidation()
        {
            ParentCriterion? parentCriterion = _parentCriteriaRepository.GetParentCriteriaByIdAndStatus(_dataRequest.ParentCriteriaId, 1);
            if(parentCriterion == null)
            {
                throw new BaseException { Messages = "Không tồn tại tiêu chí cha với ID : " + _dataRequest.ParentCriteriaId };
            }

            if(_childCriteriaRepository.ExistsByNameAndParentCriteriaId(_dataRequest.CriteriaName, parentCriterion.Id))
            {
                throw new BaseException { Messages = "Tên tiêu chí tồn tại." };
            }

            /*if(_childCriteriaRepository.ExistsByOrderIndexAndParentCriteriaId(_dataRequest.OrderIndex ?? 1, parentCriterion.Id))
            {
                throw new BaseException { Messages = "Thứ tự xuất hiện đã tồn tại." };
            }*/
        }

        public override void P3AccessDatabase()
        {
            _childCriteriaRepository.Add(newChildCriterion);
            _childCriteriaRepository.Save();
        }

        public override void P4GenerateResponseData()
        {
            if(newChildCriterion != null)
            {
                _dataResponse = new ChildCriterionResponse
                {
                    Id = newChildCriterion.Id,
                    ParentCriteriaId = newChildCriterion.ParentCriteriaId,
                    CriteriaName = newChildCriterion.CriteriaName,
                    MaxScore = newChildCriterion.MaxScore,
                    OrderIndex = newChildCriterion.OrderIndex,
                    IsActive = newChildCriterion.IsActive,
                    Note = newChildCriterion.Note,
                    CriteriaType = newChildCriterion.CriteriaType,
                    CreatedAt = newChildCriterion.CreatedAt,
                    UpdatedAt = newChildCriterion.UpdatedAt,
                };
            }
        }
    }
}
