using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.Models;

namespace DGDiemRenLuyen.Repositories.Interfaces
{
    public interface IParentCriteriaRepository : IBaseRepository<ParentCriterion>
    {
        Task<List<ParentCriterion>> GetParentCriteriaByStudentIdAsync(string studentId, Guid timeId);

        ParentCriterion? GetParentCriteriaByIdAndStatus(Guid id, int isActive);
    }
}
