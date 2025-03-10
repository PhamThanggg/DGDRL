using DGDiemRenLuyen.Models;

namespace DGDiemRenLuyen.Repositories.Interfaces
{
    public interface ITimeRepository : IBaseRepository<Time>
    {
        Time? GetCurrentTimeRecords();
    }
}
