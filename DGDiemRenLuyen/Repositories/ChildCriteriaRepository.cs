using DGDiemRenLuyen.Data;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Repositories
{
    public class ChildCriteriaRepository : BaseRepository<ChildCriterion>, IChildCriteriaRepository
    {
        private readonly SQLDRLContext _dbContext;

        public ChildCriteriaRepository(SQLDRLContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
