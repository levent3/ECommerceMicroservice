using ECommerceMicroservice.Core.Interfaces;
using ECommerceMicroservice.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceMicroservice.Infrastructure.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {

        protected readonly ApplicationDbContext _context;

        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T entity)
        {

            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
           
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
          return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
