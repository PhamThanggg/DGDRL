using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using System.Linq.Expressions;

namespace DGDiemRenLuyen.Services.ParentCriterionService
{
    public class ParentCriterionGetListService : BaseService<ParentCriterionGetListRequest, PageResponse<List<ParentCriterionResponse>>>
    {
        private readonly IParentCriteriaRepository _parentCriteriaRepository;
        private List<ParentCriterionResponse> _listparentCriterion;
        private int _totalRecords;

        public ParentCriterionGetListService(
            IParentCriteriaRepository parentCriteriaRepository,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _parentCriteriaRepository = parentCriteriaRepository;
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

            Expression<Func<ParentCriterion, bool>> searchCondition = s =>
                (string.IsNullOrEmpty(_dataRequest.CriteriaName) || (s.CriteriaName ?? "").Contains(_dataRequest.CriteriaName)) &&
                (!_dataRequest.IsActive.HasValue || s.IsActive == _dataRequest.IsActive);

            _totalRecords = _parentCriteriaRepository.GetBy(searchCondition).Count();

            _listparentCriterion = _parentCriteriaRepository
                 .GetBy(searchCondition)
                 .Select(s => new ParentCriterionResponse
                 {
                     Id = s.Id,
                     CriteriaName = s.CriteriaName,
                     MaxScore = s.MaxScore,
                     OrderIndex = s.OrderIndex,
                     IsActive = s.IsActive,
                     Note = s.Note,
                     CreatedAt = s.CreatedAt,
                     UpdatedAt = s.UpdatedAt
                 })
                 .Skip(skipRows)
                 .Take(_dataRequest.PageSize.Value)
                 .ToList();
        }

        public override void P4GenerateResponseData()
        {
            _dataResponse = new PageResponse<List<ParentCriterionResponse>>
            {
                Data = _listparentCriterion,
                PageIndex = _dataRequest.PageIndex ?? 1,
                PageSize = _dataRequest.PageSize ?? 20,
                TotalRecords = _totalRecords
            };
        }
    }
}
