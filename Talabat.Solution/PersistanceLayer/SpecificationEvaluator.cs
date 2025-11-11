using CoreLayer.Entities;
using CoreLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistanceLayer
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> InputQuery, ISpecifications<TEntity, TKey> Spec) where TEntity : BaseEntity<TKey>
        {
            var Query = InputQuery;
            if(Spec.Criteria is not null)
                Query = Query.Where(Spec.Criteria);

            if (Spec.OrderByAsc is not null)
                Query = Query.OrderBy(Spec.OrderByAsc);

            if (Spec.OrderByDesc is not null)
                Query = Query.OrderByDescending(Spec.OrderByDesc);

            if (Spec.Includes is not null && Spec.Includes.Count() > 0)
                Query = Spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExp) => CurrentQuery.Include(IncludeExp));

            if (Spec.IsPaginated)
                Query = Query.Skip(Spec.Skip).Take(Spec.Take);

            return Query;
        }
    }
}
