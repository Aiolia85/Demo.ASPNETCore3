using Entity;
using Microsoft.EntityFrameworkCore;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reoository.EF
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private DbContext _context;

        public BaseRepository(SQLiteDbContext context)
        {
            this._context = context;
        }
        public void Delete(int id)
        {
            T obj = _context.Set<T>().Find(id);
            _context.Set<T>().Remove(obj);
        }
        public void Delete(T obj)
        {
            this._context.Remove(obj);
        }

        public IEnumerable<T> GetAll()
        {
            return this._context.Set<T>().AsEnumerable();
        }

        public T GetById(int id)
        {
            return this._context.Set<T>().Find(id);
        }

        public void Insert(T obj)
        {
            this._context.Set<T>().Add(obj);
        }

        public void Save()
        {
            this._context.SaveChanges();
        }

        public void Update(T obj)
        {
            var orig = this._context.Set<T>().Find(obj.Id);
            this._context.Entry(orig).CurrentValues.SetValues(obj);
            //this._context.Entry(obj).State = EntityState.Modified;
        }
    }
}
