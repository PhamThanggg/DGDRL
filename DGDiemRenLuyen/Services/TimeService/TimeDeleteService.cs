using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Extentions;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.TimeService
{
    public class TimeDeleteService : BaseService<Guid?, string>
    {
        private readonly ITimeRepository _timeRepository;
        private readonly IScoreStatusRepository _scoreStatusRepository;


        public TimeDeleteService(
           ITimeRepository timeRepository,
           IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _timeRepository = timeRepository;
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

            Time timeDB = _timeRepository.GetById(_dataRequest);

            if ((timeDB == null) || (timeDB.Id == null))
            {
                throw new BaseException { Messages = "Id không tồn tại đúng!" };
            }

            ScoreStatus scoreStatus = _scoreStatusRepository
              .GetBy(s => s.TimeId == timeDB.Id)
              .FirstOrDefault();

            if (scoreStatus != null)
            {
                throw new BaseException { Messages = ValidationKeyWords.VALID_RFR };
            }
        }

        public override void P3AccessDatabase()
        {
            _timeRepository.Delete<Guid?>(_dataRequest);
            _timeRepository.Save();

        }

        public override void P4GenerateResponseData()
        {
            _dataResponse = ValidationKeyWords.DELETE.ToString();

        }
    }
}

