using System.Linq.Expressions;

namespace DGDiemRenLuyen.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> : IDisposable
    where TEntity : class
    {
        void Add(TEntity Entity);

        void AddRange(List<TEntity> Entities);

        IQueryable<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate);

        void Update(TEntity Entity);

        void UpdateRange(List<TEntity> entities);

        void Delete<T>(T Id);

        void Delete(TEntity Entity);

        void DeleteRange(List<TEntity> items);

        int Save();

        TEntity GetById<T>(T Id);

        IEnumerable<TEntity> All();

        bool ExistsBy(Expression<Func<TEntity, bool>> predicate);
    }
}
