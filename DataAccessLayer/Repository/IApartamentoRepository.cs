using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public interface IApartamentoRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();

        Task<TEntity> GetEntityById(int id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);
    }
}
