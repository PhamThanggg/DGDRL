using DGDiemRenLuyen.Data;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DGDiemRenLuyen.Repositories
{
    public class TimeRepository : BaseRepository<Time>, ITimeRepository
    {
        private readonly SQLDRLContext _dbContext;

        public TimeRepository(SQLDRLContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Time? GetCurrentTimeRecords()
        {
            DateTime now = DateTime.UtcNow.AddHours(7).Date;

            return _dbContext.Set<Time>()
                .Where(t => t.StartDate <= now && t.EndDate >= now)
                .OrderBy(t => t.StartDate)
                .FirstOrDefault();

        }
    }
}
