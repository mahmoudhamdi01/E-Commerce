using CoreLayer.Entities;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Specifications
{
    public class ProductCountSpec : BaseSpecification<Product, int>
    {
        public ProductCountSpec(ProductQueryParams queryParams) :
            base(P => (!queryParams.BrandId.HasValue || P.BrandId == queryParams.BrandId)
            && (!queryParams.TypeId.HasValue || P.TypeId == queryParams.TypeId) &&
            (string.IsNullOrWhiteSpace(queryParams.SearchValue) || P.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))
        {

        }
    }
}
