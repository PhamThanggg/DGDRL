using DGDiemRenLuyen.Models;

namespace DGDiemRenLuyen.Repositories.Interfaces
{
    public interface IScoreStatusRepository : IBaseRepository<ScoreStatus>
    {
        bool ExistsByStudentIdAndTimeId(String studentId, Guid timeId);

        ScoreStatus FindByStudentIdAndTimeId(String studentId, Guid timeId);

        List<ScoreStatus> FindInStudentId(List<string> lists, Guid? timeId);

        List<ScoreStatus> GetInIdAndStatus(List<Guid> ids, int status);
    }
}
