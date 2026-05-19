using ECommerce.Interface.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Interface.IServices.Product
{
    public interface IProductService
    {
        Task<PagedResult<ProductReadDTO>> GetAllProductsAsync(ProductQueryParams queryParams);
        Task<PagedResult<ProductReadDTO>> GetProductsByBrandId(int brandId, ProductQueryParams queryParams);
        //Task<IEnumerable<ProductReadDTO>> GetAllProductsAsync();
        Task<ProductReadDTO> GetProductById(int id);
        Task<ProductReadDTO> AddProductAsync(ProductCreateUpdateDTO productCreateUpdateDTO);
        Task<ProductReadDTO> UpdateProductAsync(int id, ProductCreateUpdateDTO productCreateUpdateDTO);
        Task<bool> DeleteProductAsync(int id);
        //Task<IEnumerable<ProductReadDTO>> GetProductsByBrandId(int brandId);
    }
}
