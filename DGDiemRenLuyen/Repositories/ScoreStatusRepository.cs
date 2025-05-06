using Azure.Core;
using DGDiemRenLuyen.Data;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

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

        public List<ScoreStatus> FindInStudentId(List<string> lists, Guid? timeId)
        {
            if (lists == null || lists.Count == 0)
            {
                return new List<ScoreStatus>();
            }

            return _dbContext.ScoreStatus
                             .Where(ss => lists.Contains(ss.StudentId) && ss.TimeId == timeId)
                             .ToList();
        }

        public List<ScoreStatus> GetInIdAndStatus(List<Guid> ids, int status)
        {
            return _dbContext.ScoreStatus
               .Where(e => ids.Contains(e.Id) && e.Status == status)
               .ToList();
        }
    }
}
