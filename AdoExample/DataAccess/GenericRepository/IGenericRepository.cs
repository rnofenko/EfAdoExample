using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace AdoExample.DataAccess.GenericRepository
{
    public interface IGenericRepository<T> : IDisposable where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindAllBy(Expression<Func<T, bool>> predicate);
        T FindFirstBy(Expression<Func<T, bool>> predicate);
        T Find(int id);
        void Add(T entity);
        void Delete(T entity);
        void Delete(IQueryable<T> entities);
        void Edit(T entity);
        int Save();
        int Count();
        void Refresh(T entity);
    }
}