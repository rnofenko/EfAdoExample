using System;
using System.Data.Entity.Validation;
using System.Linq;

namespace AdoExample.DataAccess.GenericRepository
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public EfContext Context { get; set; }

        protected GenericRepository()
        {
            Context = new EfContext();
        }

        protected GenericRepository(EfContext context)
        {
            Context = context;
        }

        public virtual IQueryable<T> GetAll()
        {
            return Context.Set<T>();
        }

        public virtual IQueryable<T> FindAllBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().Where(predicate);
        }

        public virtual T FindFirstBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().FirstOrDefault(predicate);
        }

        public virtual void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public virtual void Delete(IQueryable<T> entities)
        {
            foreach (T entity in entities.ToList())
                Context.Set<T>().Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            Context.Entry(entity).State = System.Data.EntityState.Modified;
        }

        public virtual int Save()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName,validationError.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Save failed", ex);
            }
            return 0;
        }

        public virtual T Find(int id)
        {
            return Context.Set<T>().Find(id);
        }

        public void Refresh(T entity)
        {
            Context.Entry(entity).Reload();
        }

        public virtual int Count()
        {
            return Context.Set<T>().Count();
        }

        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
        }
    }
}