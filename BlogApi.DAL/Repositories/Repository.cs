using BlogApi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly BlogApiContext _blogApiContext;
        private readonly DbSet<T> _dbset;
        public Repository(BlogApiContext blogApiContext)
        {
            _blogApiContext = blogApiContext;
            _dbset = blogApiContext.Set<T>();
        }

        public async Task Add(T entity)
        {
             await _dbset.AddAsync(entity);
        }

        public async Task<List<T>> Get()
        {
            return await _dbset.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public void Remove(T entity)
        {
            _dbset.Remove(entity);
        }

        public async Task Save()
        {
            await _blogApiContext.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _dbset.Update(entity);
        }

    }
}
