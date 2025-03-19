using DGDiemRenLuyen.Data;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace DGDiemRenLuyen.Repositories
{
    public class ParentCriteriaRepository : BaseRepository<ParentCriterion>, IParentCriteriaRepository
    {
        private readonly SQLDRLContext _dbContext;

        public ParentCriteriaRepository(SQLDRLContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public ParentCriterion? GetParentCriteriaByIdAndStatus(Guid id, int isActive)
        {
            return _dbContext.ParentCriteria
                .FirstOrDefault(x => x.Id == id && x.IsActive == isActive);
        }

        public async Task<List<ParentCriterion>> GetParentCriteriaByStudentIdAsync(String studentId, Guid timeId)
        {
            var parentCriteria = await _dbContext.ParentCriteria
                 .Where(p => p.IsActive == 1)
                 .ToListAsync(); // Lấy tất cả ParentCriteria

            var childCriteria = await _dbContext.ChildCriteria
                .Where(c => c.IsActive == 1 && parentCriteria.Select(p => p.Id).Contains(c.ParentCriteriaId))
                .ToListAsync(); // Lấy ChildCriteria có Parent

            var criteriaDetails = await _dbContext.CriteriaDetails
                .Where(cd => childCriteria.Select(c => c.Id).Contains(cd.ChildCriteriaId))
                .ToListAsync(); // Lấy CriteriaDetails có ChildCriteria

            var scoreStatuses = await _dbContext.ScoreStatus
                .Where(ss => criteriaDetails.Select(cd => cd.Id).Contains(ss.Id))
                .ToListAsync(); // Lấy ScoreStatus dựa trên CriteriaDetails

            // Gán dữ liệu theo LEFT JOIN
            foreach (var child in childCriteria)
            {
                child.CriteriaDetails = criteriaDetails
                    .Where(cd => cd.ChildCriteriaId == child.Id)
                    .Select(cd =>
                    {
                        cd.ScoreStatus = scoreStatuses.FirstOrDefault(ss => ss.Id == cd.Id);
                        return cd;
                    })
                    .ToList();
            }

            foreach (var parent in parentCriteria)
            {
                parent.ChildCriteria = childCriteria.Where(c => c.ParentCriteriaId == parent.Id).ToList();
            }

            return parentCriteria;
        }
    }
}
