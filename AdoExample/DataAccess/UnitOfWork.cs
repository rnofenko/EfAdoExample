using System;

namespace AdoExample.DataAccess
{
    public class UnitOfWork
    {
        private readonly EfContext _context;

        public UnitOfWork()
        {
            _context = new EfContext();
        }

        public T GetRepository<T>()
        {
            var repository = Activator.CreateInstance(typeof (T), _context);
            return (T)repository;
        }
    }
}
