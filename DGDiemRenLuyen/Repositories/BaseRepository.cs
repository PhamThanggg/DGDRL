using DGDiemRenLuyen.Data;
using DGDiemRenLuyen.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DGDiemRenLuyen.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly SQLDRLContext _dbContext;

        public BaseRepository(SQLDRLContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(TEntity Entity)
        {
            _dbContext.Set<TEntity>().Add(Entity);
        }

        public void AddRange(List<TEntity> Entities)
        {
            _dbContext.Set<TEntity>().AddRange(Entities);
        }

        public IEnumerable<TEntity> All()
        {
            return _dbContext.Set<TEntity>();
        }

        public void Delete<T>(T Id)
        {
            TEntity Entity = _dbContext.Set<TEntity>().Find(Id);

            if (Entity != null)
            {
                _dbContext.Set<TEntity>().Remove(Entity);
            }
        }

        public void Delete(TEntity Entity)
        {
            _dbContext.Set<TEntity>().Remove(Entity);
        }

        public void DeleteRange(List<TEntity> items)
        {
            _dbContext.Set<TEntity>().RemoveRange(items);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IQueryable<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().Where(predicate);
        }

        public TEntity GetById<T>(T Id)
        {
            return _dbContext.Set<TEntity>().Find(Id);
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        public void Update(TEntity Entity)
        {
            _dbContext.Set<TEntity>().Attach(Entity);
            _dbContext.Entry(Entity).State = EntityState.Modified;
        }

        public void UpdateRange(List<TEntity> entities)
        {
            _dbContext.Set<TEntity>().UpdateRange(entities);
        }

        public bool ExistsBy(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().Any(predicate);
        }
    }
}
