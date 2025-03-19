﻿using DGDiemRenLuyen.Data;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Repositories
{
    public class CriteriaDetailRepository : BaseRepository<CriteriaDetail>, ICriteriaDetailRepository
    {
        private readonly SQLDRLContext _dbContext;

        public CriteriaDetailRepository(SQLDRLContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public CriteriaDetail? FindByChildCriterieIdAndScoreId(Guid childCrteriaId, Guid scoreId)
        {
            return _dbContext.CriteriaDetails
                .FirstOrDefault(x => x.ChildCriteriaId == childCrteriaId && x.ScoreId == scoreId);
        }

        public List<CriteriaDetail>? FindByScoreIdAndChildCriteriaParentCriterieId(Guid scoreStatusId, Guid parentCriteriaId)
        {
            return _dbContext.CriteriaDetails
                .Where(cd => cd.ScoreId == scoreStatusId && cd.ChildCriteria.ParentCriteriaId == parentCriteriaId)
                .ToList();
        }
    }
}
