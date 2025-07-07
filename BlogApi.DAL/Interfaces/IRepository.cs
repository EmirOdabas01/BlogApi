using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> Get();
        Task<T> GetById(int id);
        Task Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task Save();
    }
}
