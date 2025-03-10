using DGDiemRenLuyen.Data;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Repositories
{
    public class ScoreStatusRepository : BaseRepository<ScoreStatus>, IScoreStatusRepository
    {
        private readonly SQLDRLContext _dbContext;

        public ScoreStatusRepository(SQLDRLContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public bool ExistsByStudentIdAndTimeId(string studentId, Guid timeId)
        {
            return _dbContext.ScoreStatus
                .Any(x => x.StudentId == studentId && x.TimeId == timeId);
        }

        public ScoreStatus? FindByStudentIdAndTimeId(string studentId, Guid timeId)
        {
            return _dbContext.ScoreStatus
                .FirstOrDefault(x => x.StudentId == studentId && x.TimeId == timeId);
        }
    }
}
