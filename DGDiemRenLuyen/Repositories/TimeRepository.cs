using DGDiemRenLuyen.Data;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Repositories
{
    public class TimeRepository : BaseRepository<Time>, ITimeRepository
    {
        private readonly SQLDRLContext _dbContext;

        public TimeRepository(SQLDRLContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
