using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.Models;

namespace DGDiemRenLuyen.Repositories.Interfaces
{
    public interface IChildCriteriaRepository : IBaseRepository<ChildCriterion>
    {
        ChildCriterion? GetChildCriteriaByIdAndStatus(Guid id, int isActive);

        bool ExistsByNameAndParentCriteriaId(string name, Guid parentCriteriaId);

        bool ExistsByOrderIndexAndParentCriteriaId(int orderIndex, Guid parentCriteriaId);
    }
}
