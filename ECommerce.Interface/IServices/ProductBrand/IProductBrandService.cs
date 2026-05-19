using ECommerce.Interface.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Interface.IServices.ProductBrand
{
    public interface IProductBrandService
    {
        Task<PagedResult<ProductBrandReadDTO>> GetAllAsync(ProductBrandQueryParams queryParams);
        Task<ProductBrandReadDTO> GetByIdAsync(int id);
        Task<ProductBrandReadDTO> AddAsync(ProductBrandCreateDTO dto);
        Task<ProductBrandReadDTO> UpdateAsync(int id, ProductBrandCreateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
