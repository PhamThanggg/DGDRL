﻿using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.ScoreStatusService
{
    public class ScoreStatusDeleteService : BaseService<Guid?, string>
    {
        private readonly IScoreStatusRepository _scoreStatusRepository;


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

            ScoreStatus parentCriterion = _scoreStatusRepository.GetById(_dataRequest);

            if ((parentCriterion == null) || (parentCriterion.Id == null))
            {
                throw new BaseException { Messages = "Id không tồn tại đúng!" };
            }
        }

        public override void P3AccessDatabase()
        {
            _scoreStatusRepository.Delete<Guid?>(_dataRequest);
            _scoreStatusRepository.Save();

        }

        public override void P4GenerateResponseData()
        {
            _dataResponse = "Xóa thành công!";

        }
    }
}

