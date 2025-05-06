using Azure.Core;
using DGDiemRenLuyen.DTOs.Requests;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.Extentions;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net.NetworkInformation;


namespace DGDiemRenLuyen.Services.ScoreStatusService
{
    public class ScoreStatusUpdateListService : BaseService<StatusUpdateListRequest, IActionResult>
    {
        private readonly IScoreStatusRepository _scoreStatusRepository;
        private List<ScoreStatus>? _updatedIds = new();
        private List<Guid>? _notMatchedIds = new();

        public ScoreStatusUpdateListService(
            IScoreStatusRepository scoreStatusRepository,
            IHttpContextAccessor httpContextAccessor,
            string successMessageDefault = ValidationKeyWords.UPDATE) : base(httpContextAccessor, successMessageDefault)
        {
            _scoreStatusRepository = scoreStatusRepository;
        }

        public override void P1GenerateObjects()
        {
            int statusDB = _dataRequest.Status > 0 ? _dataRequest.Status - 1 : 0;
            _updatedIds = _scoreStatusRepository.GetInIdAndStatus(_dataRequest.Ids, statusDB);

            foreach (var entity in _updatedIds)
            {
                entity.Status = _dataRequest.Status;
            }

            var update = _updatedIds.Select(e => e.Id).ToList();
            _notMatchedIds = _dataRequest.Ids.Except(update).ToList();
        }

        public override void P2PostValidation()
        {
        }

        public override void P3AccessDatabase()
        {
            if(_updatedIds != null)
            {
                _scoreStatusRepository.UpdateRange(_updatedIds);
                _scoreStatusRepository.Save();
            }
        }

        public override void P4GenerateResponseData()
        {
            _dataResponse = new OkObjectResult(new
            {
                Updated = _updatedIds.Count,
                UpdatedIds = _updatedIds.Select(e => e.Id),
                SkippedIds = _notMatchedIds
            });
        }
    }
}
