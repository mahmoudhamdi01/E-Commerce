using CoreLayer.Entities;
using SharedLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Specifications
{
    public class ProductWithBrandAndTypeSpec : BaseSpecification<Product, int>
    {
        public ProductWithBrandAndTypeSpec(ProductQueryParams queryParams) :
            base(P=>(!queryParams.BrandId.HasValue || P.BrandId == queryParams.BrandId)
            && (!queryParams.TypeId.HasValue || P.TypeId == queryParams.TypeId) &&
            (string.IsNullOrWhiteSpace(queryParams.SearchValue) || P.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))
        {
            AddIncludes(P => P.ProductType);
            AddIncludes(P => P.ProductBrand);

            switch (queryParams.SortingOptions)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(P => P.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDesc(P => P.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(P => P.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDesc(P => P.Price);
                    break;
                default:
                    break;
            }

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }

        public ProductWithBrandAndTypeSpec(int id) : base(P=>P.Id == id)
        {
            AddIncludes(P => P.ProductType);
            AddIncludes(P => P.ProductBrand);
        }
    }
}
