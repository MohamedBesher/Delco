using Saned.Delco.Data.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Delco.Data.Persistence.Repositories
{
    public class BaseRepository<TObject> : IBaseRepository<TObject>
      where TObject : class

    {


        protected ApplicationDbContext _context = null;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        protected DbSet<TObject> DbSet
        {
            get
            {
                return _context.Set<TObject>();
            }
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public virtual IQueryable<TObject> All()
        {
            return DbSet.AsQueryable();
        }

        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Where(predicate).AsQueryable<TObject>();
        }

        public virtual IQueryable<TObject> Filter(Expression<Func<TObject, bool>> filter, out int total, int index = 0, int size = 50)
        {
            int skipCount = index * size;
            var _resetSet = filter != null ? DbSet.Where(filter).AsQueryable() : DbSet.AsQueryable();
            _resetSet = skipCount == 0 ? _resetSet.Take(size) : _resetSet.Skip(skipCount).Take(size);
            total = _resetSet.Count();
            return _resetSet.AsQueryable();
        }

        public bool Contains(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.Count(predicate) > 0;
        }

        public virtual TObject Find(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public virtual TObject Find(Expression<Func<TObject, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public virtual TObject Create(TObject TObject)
        {
            var newEntry = DbSet.Add(TObject);
            return newEntry;
        }

        public virtual int Count
        {
            get
            {
                return DbSet.Count();
            }
        }

        public virtual void Delete(TObject TObject)
        {
            DbSet.Remove(TObject);
        }

        public virtual void Update(TObject TObject)
        {
            var entry = _context.Entry(TObject);
            DbSet.Attach(TObject);
            entry.State = EntityState.Modified;
        }
        public virtual TObject Updated(TObject TObject)
        {
          
            var entry = _context.Entry(TObject);
           var newEntry= DbSet.Attach(TObject);
            entry.State = EntityState.Modified;
            return newEntry;
        }

        public virtual void Delete(Expression<Func<TObject, bool>> predicate)
        {
            var objects = Filter(predicate);
            foreach (var obj in objects)
                DbSet.Remove(obj);
        }
    }
}
