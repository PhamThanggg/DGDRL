using DGDiemRenLuyen.Models;
using Microsoft.EntityFrameworkCore;

namespace DGDiemRenLuyen.Repositories.Interfaces
{
    public interface IRoleAssignmentRepository : IBaseRepository<RoleAssignment>
    {
        RoleAssignment? GetByObjectId(string Id);
    }
}
