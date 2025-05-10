using DGDiemRenLuyen.Models;

namespace DGDiemRenLuyen.Repositories.Interfaces
{
    public interface IScoreStatusRepository : IBaseRepository<ScoreStatus>
    {
        bool ExistsByStudentIdAndTimeId(String studentId, Guid timeId);

        ScoreStatus FindByStudentIdAndTimeId(String studentId, Guid timeId);

        List<ScoreStatus> FindInStudentId(List<string> lists, Guid? timeId);

        List<ScoreStatus> GetInIdAndStatus(List<Guid> ids, int status);

        List<ScoreStatus> GetInIdAndStatus(List<Guid> ids, int status, HashSet<string> listClass);

        List<ScoreStatus> GetInIdAndStudentClassIdAndStatus(List<Guid> ids, string classStudentID, int status);

        List<ScoreStatus> GetInIdAndDepartmentIdAStdatus(List<Guid> ids, string departmentID, int status);

        ScoreStatus? FindById(Guid id);
    }
}
