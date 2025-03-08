using DGDiemRenLuyen.Data;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Repositories
{
    public class ParentCriteriaRepository : BaseRepository<ParentCriterion>, IParentCriteriaRepository
    {
        private readonly SQLDRLContext _dbContext;

        public ParentCriteriaRepository(SQLDRLContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
