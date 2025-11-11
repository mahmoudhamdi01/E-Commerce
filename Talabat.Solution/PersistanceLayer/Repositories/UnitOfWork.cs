using CoreLayer.Entities;
using CoreLayer.Interfaces;
using PersistanceLayer.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistanceLayer.Repositories
{
    public class UnitOfWork(ApplicationDbContext _dbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repository = [];
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var TypeName = typeof(TEntity).Name;
            if(_repository.ContainsKey(TypeName))
                return (IGenericRepository<TEntity, TKey>)_repository[TypeName];
            else
            {
                var Repo = new GenericRepository<TEntity, TKey>(_dbContext);
                _repository.Add(TypeName, Repo);
                return Repo;
            }
        }

        public async Task<int> SaveChangesAsync()
        => await _dbContext.SaveChangesAsync();
    }
}
