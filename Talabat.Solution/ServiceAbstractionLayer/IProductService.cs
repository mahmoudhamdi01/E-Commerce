using SharedLayer;
using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstractionLayer
{
    public interface IProductService
    {
        Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParams queryParams);
        Task<ProductDTO> GetProductByIdAsync(int id);
        Task<IEnumerable<BrandDTO>> GetAllBrandsAsync();
        Task<IEnumerable<TypeDTO>> GetAllTypesAsync();
    }
}
