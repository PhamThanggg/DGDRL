using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using System;
using System.Linq.Expressions;

namespace DGDiemRenLuyen.Services.TimeService
{
    public class TimeGetListService : BaseService<TimeGetListRequest, PageResponse<List<TimeResponse>>>
    {
        private readonly ITimeRepository _timeRepository;
        private List<TimeResponse> _listparentCriterion;
        private int _totalRecords;

        public TimeGetListService(
            ITimeRepository timeRepository,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _timeRepository = timeRepository;
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

            Expression<Func<Time, bool>> searchCondition = s =>
                (!_dataRequest.StartYear.HasValue || s.StartYear == _dataRequest.StartYear) &&
                (string.IsNullOrEmpty(_dataRequest.TermID) || s.TermID == _dataRequest.TermID);

            _totalRecords = _timeRepository.GetBy(searchCondition).Count();

            _listparentCriterion = _timeRepository
                 .GetBy(searchCondition)
                 .Select(s => new TimeResponse
                 {
                     Id = s.Id,
                     TermID = s.TermID,
                     StartYear = s.StartYear,
                     EndYear = s.StartYear + 1,
                     StartDate = s.StartDate,
                     EndDate = s.EndDate,
                     CreatedBy = s.CreatedBy,
                     CreatedAt = s.CreatedAt,
                     UpdatedAt = s.UpdatedAt,
                 })
                 .Skip(skipRows)
                 .Take(_dataRequest.PageSize.Value)
                 .ToList();
        }

        public override void P4GenerateResponseData()
        {
            _dataResponse = new PageResponse<List<TimeResponse>>
            {
                Data = _listparentCriterion,
                PageIndex = _dataRequest.PageIndex ?? 1,
                PageSize = _dataRequest.PageSize ?? 20,
                TotalRecords = _totalRecords
            };
        }
    }
}
