using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Extentions;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.ScoreStatusService
{
    public class ScoreStatusDeleteService : BaseService<Guid?, string>
    {
        private readonly IScoreStatusRepository _scoreStatusRepository;
        private readonly ICriteriaDetailRepository _criteriaDetailRepository;

        public ScoreStatusDeleteService(
           IScoreStatusRepository scoreStatusRepository,
           IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _scoreStatusRepository = scoreStatusRepository;
        }

        public override void P1GenerateObjects()
        {
            
        }

        public override void P2PostValidation()
        {
            if (_dataRequest == null)
            {
                throw new BaseException { Messages = "Id không đúng!" };
            }

            ScoreStatus scoreStatus = _scoreStatusRepository.GetById(_dataRequest);

            if ((scoreStatus == null) || (scoreStatus.Id == null))
            {
                throw new BaseException { Messages = "Id không tồn tại đúng!" };
            }

            CriteriaDetail criteriaDetail = _criteriaDetailRepository
              .GetBy(s => s.ChildCriteriaId == scoreStatus.Id)
              .FirstOrDefault();

            if (criteriaDetail != null)
            {
                throw new BaseException { Messages = ValidationKeyWords.VALID_RFR };
            }
        }

        public override void P3AccessDatabase()
        {
            _scoreStatusRepository.Delete<Guid?>(_dataRequest);
            _scoreStatusRepository.Save();

        }

        public override void P4GenerateResponseData()
        {
            _dataResponse = ValidationKeyWords.DELETE.ToString();

        }
    }
}

