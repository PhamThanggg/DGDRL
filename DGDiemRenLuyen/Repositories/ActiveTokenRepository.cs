using DGDiemRenLuyen.Data;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DGDiemRenLuyen.Repositories
{
    public class ActiveTokenRepository : BaseRepository<ActiveToken>, IActiveTokenRepository
    {
        private readonly SQLDRLContext _dbContext;

        public ActiveTokenRepository(SQLDRLContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
