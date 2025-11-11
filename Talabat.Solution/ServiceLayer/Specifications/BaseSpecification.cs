using CoreLayer.Entities;
using CoreLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Specifications
{
    public class BaseSpecification<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public BaseSpecification(Expression<Func<TEntity, bool>>? CriteriaExpression)
        {
            Criteria = CriteriaExpression;
        }
        public Expression<Func<TEntity, bool>>? Criteria { get; set; }
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = [];
        public Expression<Func<TEntity, object>> OrderByAsc { get; set; }
        public Expression<Func<TEntity, object>> OrderByDesc { get; set; }
    
        public void AddIncludes(Expression<Func<TEntity, object>> IncludeExpression)
        {
            Includes.Add(IncludeExpression);
        }

        public void AddOrderBy(Expression<Func<TEntity, object>> OrderByExp)
        {
            OrderByAsc = OrderByExp;
        }

        public void AddOrderByDesc(Expression<Func<TEntity, object>> OrderByDescExp)
        {
            OrderByDesc = OrderByDescExp;
        }

        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginated { get; set; }

        public void ApplyPagination(int PageSize, int PageIndex)
        {
            IsPaginated = true;
            Take = PageSize;
            Skip = (PageIndex - 1) * PageSize;
        }
    }
}
