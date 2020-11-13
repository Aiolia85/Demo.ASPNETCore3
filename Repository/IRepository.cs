using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public interface IRepository
    {
    }

    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        T GetById(int id);

        void Insert(T obj);

        void Update(T obj);

        void Delete(T obj);

        void Delete(int id);

        void Save();
    }
}
