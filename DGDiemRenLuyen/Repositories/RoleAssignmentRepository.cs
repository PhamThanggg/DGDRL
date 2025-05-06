using DGDiemRenLuyen.Data;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DGDiemRenLuyen.Repositories
{
    public class RoleAssignmentRepository : BaseRepository<RoleAssignment>, IRoleAssignmentRepository
    {
        private readonly SQLDRLContext _dbContext;

        public RoleAssignmentRepository(SQLDRLContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public RoleAssignment? GetByObjectId(string Id)
        {
            return _dbContext.RoleAssignments
                        .FirstOrDefault(c => c.ObjectId == Id);
        }
    }
}
