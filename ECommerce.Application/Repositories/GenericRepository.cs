using ECommerce.Infrastructure.Contexts;
using ECommerce.Infrastructure.Entities;
using ECommerce.Interface.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories
{
    public class GenericRepository<TEntity, TKey>(ApplicationDbContext _dbContext) : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public IQueryable<TEntity> Query()
        => _dbContext.Set<TEntity>().AsQueryable();

        public async Task AddAsync(TEntity entity)
        => await _dbContext.AddAsync(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        => await _dbContext.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(int id)
        => await _dbContext.Set<TEntity>().FindAsync(id);

        public void Remove(TEntity entity)
        => _dbContext.Remove(entity);

        public void Update(TEntity entity)
        => _dbContext.Update(entity);
    }
}
