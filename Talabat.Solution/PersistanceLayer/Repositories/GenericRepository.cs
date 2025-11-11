using CoreLayer.Entities;
using CoreLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using PersistanceLayer.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistanceLayer.Repositories
{
    public class GenericRepository<TEntity, TKey>(ApplicationDbContext _dbContext) : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public async Task AddAsync(TEntity entity)
        => await _dbContext.AddAsync(entity);

        public void Delete(TEntity entity)
        => _dbContext.Remove(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        => await _dbContext.Set<TEntity>().ToListAsync();
        public async Task<TEntity?> GetByIdAsync(TKey id)
        => await _dbContext.Set<TEntity>().FindAsync(id);

        public void Update(TEntity entity)
        => _dbContext.Update(entity);

        #region With Specification
        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> Spec)
       => await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), Spec).ToListAsync();

        public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, TKey> Spec)
        => await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), Spec).FirstOrDefaultAsync();

        public async Task<int> CountAsync(ISpecifications<TEntity, TKey> Spec)
        => await SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), Spec).CountAsync();

        #endregion

    }
}
