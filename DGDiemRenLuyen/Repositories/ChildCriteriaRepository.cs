using DGDiemRenLuyen.Data;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DGDiemRenLuyen.Repositories
{
    public class ChildCriteriaRepository : BaseRepository<ChildCriterion>, IChildCriteriaRepository
    {
        private readonly SQLDRLContext _dbContext;

        public ChildCriteriaRepository(SQLDRLContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public bool ExistsByNameAndParentCriteriaId(string name, Guid parentCriteriaId)
        {
            return _dbContext.ChildCriteria
                .Any(cd => cd.CriteriaName == name && cd.ParentCriteriaId == parentCriteriaId);
        }

        public bool ExistsByOrderIndexAndParentCriteriaId(int orderIndex, Guid parentCriteriaId)
        {
            return _dbContext.ChildCriteria
                .Any(cd => cd.OrderIndex == orderIndex && cd.ParentCriteriaId == parentCriteriaId);
        }

        public ChildCriterion? GetChildCriteriaByIdAndStatus(Guid id, int isActive)
        {
            return _dbContext.ChildCriteria
                        .Include(cc  => cc.ParentCriteria)
                        .FirstOrDefault(c => c.Id == id && c.IsActive == isActive);
        }

        public void UpdateIsActiveByParentCriteriaId(int? isActive, Guid parentCriteriaId)
        {
            _dbContext.ChildCriteria
               .Where(c => c.ParentCriteriaId == parentCriteriaId)
               .ExecuteUpdate(setters => setters.SetProperty(c => c.IsActive, isActive));
        }
    }
}
