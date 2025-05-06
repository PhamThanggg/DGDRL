using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using System.Linq.Expressions;

namespace DGDiemRenLuyen.Services.ScoreStatusService
{
    public class ScoreStatusGetMyListService : BaseService<ScoreStatusGetListRequest, PageResponse<List<ScoreStatusResponse>>>
    {
        private readonly IScoreStatusRepository _scoreStatusRepository;
        private List<ScoreStatusResponse> _listparentCriterion;
        private int _totalRecords;

        public ScoreStatusGetMyListService(
            IScoreStatusRepository scoreStatusRepository,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _scoreStatusRepository = scoreStatusRepository;
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

            Expression<Func<ScoreStatus, bool>> searchCondition = s =>
                (s.StudentId == UserID) &&
                (!_dataRequest.TimeId.HasValue || s.TimeId == _dataRequest.TimeId);

            _totalRecords = _scoreStatusRepository.GetBy(searchCondition).Count();

            _listparentCriterion = _scoreStatusRepository
                 .GetBy(searchCondition)
                 .Select(s => new ScoreStatusResponse
                 {
                     ScoreStatusId = s.Id,
                     StudentID = s.StudentId,
                     TimeId = s.TimeId,
                     Status = s.Status,
                     SeductedPoint = s.SeductedPoint,
                     PlusPoint = s.PlusPoint,
                     Note = s.Note,
                     CreatedAt = s.CreatedAt,
                     UpdatedAt = s.UpdatedAt,
                 })
                 .Skip(skipRows)
                 .Take(_dataRequest.PageSize.Value)
                 .ToList();
        }

        public override void P4GenerateResponseData()
        {
            _dataResponse = new PageResponse<List<ScoreStatusResponse>>
            {
                Data = _listparentCriterion,
                PageIndex = _dataRequest.PageIndex ?? 1,
                PageSize = _dataRequest.PageSize ?? 20,
                TotalRecords = _totalRecords
            };
        }
    }
}
