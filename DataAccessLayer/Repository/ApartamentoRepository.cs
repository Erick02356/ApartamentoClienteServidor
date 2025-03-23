using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class ApartamentoRepository<TEntity> : IApartamentoRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<TEntity> _dbSet;

        public ApartamentoRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();

        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
;           }
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetEntityById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

 
    }
}
