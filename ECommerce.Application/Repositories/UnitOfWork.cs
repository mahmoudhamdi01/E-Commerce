using ECommerce.Infrastructure.Contexts;
using ECommerce.Infrastructure.Entities;
using ECommerce.Interface.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories
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
                var repo = new GenericRepository<TEntity, TKey>(_dbContext);
                _repository.Add(TypeName, repo);
                return repo;
            }
        }

        public async Task<int> SaveChangesAsync()
        => await _dbContext.SaveChangesAsync();
    }
}
