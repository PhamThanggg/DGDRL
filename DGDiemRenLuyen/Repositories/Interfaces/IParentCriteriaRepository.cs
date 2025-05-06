using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;

namespace DGDiemRenLuyen.Repositories.Interfaces
{
    public interface IParentCriteriaRepository : IBaseRepository<ParentCriterion>
    {
        Task<List<ParentCriterionDto>> GetParentCriteriaByStudentIdAsync(Guid scoreId, Guid timeId);

        ParentCriterion? GetParentCriteriaByIdAndStatus(Guid id, int isActive);
    }
}
