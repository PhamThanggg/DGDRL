using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using System.Linq.Expressions;

namespace DGDiemRenLuyen.Services.ChildCriterionService
{
    public class ChildCriterionGetListService : BaseService<ChildCriterionGetListRequest, PageResponse<List<ChildCriterionResponse>>>
    {
        private readonly IChildCriteriaRepository _childCriteriaRepository;
        private List<ChildCriterionResponse> _listchildCriterion;
        private int _totalRecords;

        public ChildCriterionGetListService(
            IChildCriteriaRepository childCriteriaRepository,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _childCriteriaRepository = childCriteriaRepository;
        }

        public override void P1GenerateObjects()
        {
            _dataRequest.PageIndex = _dataRequest.PageIndex ?? 1;
            _dataRequest.PageSize = _dataRequest.PageSize ?? 20;
        }

        public override void P2PostValidation()
        {
            
        }

        public override void P3AccessDatabase()
        {
            int skipRows = _dataRequest.PageSize.Value * (_dataRequest.PageIndex.Value - 1);

            Expression<Func<ChildCriterion, bool>> searchCondition = s =>
                (string.IsNullOrEmpty(_dataRequest.CriteriaName) || (s.CriteriaName ?? "").Contains(_dataRequest.CriteriaName)) &&
                (!_dataRequest.IsActive.HasValue || s.IsActive == _dataRequest.IsActive) &&
                (!_dataRequest.CriteriaType.HasValue || s.CriteriaType == _dataRequest.CriteriaType) &&
                (!_dataRequest.ParentCriteriaId.HasValue || s.ParentCriteriaId == _dataRequest.ParentCriteriaId);

            var query = _childCriteriaRepository
                 .GetBy(searchCondition)
                 .Select(s => new ChildCriterionResponse
                 {
                     Id = s.Id,
                     ParentCriteriaId = s.ParentCriteriaId,
                     CriteriaName = s.CriteriaName,
                     MaxScore = s.MaxScore,
                     OrderIndex = s.OrderIndex,
                     IsActive = s.IsActive,
                     Note = s.Note,
                     CriteriaType = s.CriteriaType,
                     CreatedAt = s.CreatedAt,
                     UpdatedAt = s.UpdatedAt
                 })
                 .AsQueryable();

            _totalRecords = query.Count();

            _listchildCriterion = query
            .Skip(skipRows)
            .Take(_dataRequest.PageSize.Value)
            .ToList();
        }

        public override void P4GenerateResponseData()
        {
            _dataResponse = new PageResponse<List<ChildCriterionResponse>>
            {
                Data = _listchildCriterion,
                PageIndex = _dataRequest.PageIndex ?? 1,
                PageSize = _dataRequest.PageSize ?? 20,
                TotalRecords = _totalRecords
            };
        }
    }
}
