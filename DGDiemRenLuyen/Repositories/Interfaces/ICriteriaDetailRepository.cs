using DGDiemRenLuyen.Models;

namespace DGDiemRenLuyen.Repositories.Interfaces
{
    public interface ICriteriaDetailRepository : IBaseRepository<CriteriaDetail>
    {
        CriteriaDetail? FindByChildCriterieIdAndScoreId(Guid childCrteriaId, Guid scoreId);
    }
}
