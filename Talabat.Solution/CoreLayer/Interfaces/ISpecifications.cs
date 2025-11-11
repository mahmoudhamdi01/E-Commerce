using CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Interfaces
{
    public interface ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; set; }
        public List<Expression<Func<TEntity, object>>> Includes { get; set; }

        public Expression<Func<TEntity, object>> OrderByAsc { get; set; }
        public Expression<Func<TEntity, object>> OrderByDesc { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginated { get; set; }
    }
}
