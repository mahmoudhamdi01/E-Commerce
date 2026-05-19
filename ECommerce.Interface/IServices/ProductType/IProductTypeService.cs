using ECommerce.Interface.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Interface.IServices.ProductType
{
    public interface IProductTypeService
    {
        Task<PagedResult<ProductTypeReadDTO>> GetAllAsync(ProductTypeQueryParams queryParams);
        Task<ProductTypeReadDTO> GetByIdAsync(int id);
        Task<ProductTypeReadDTO> AddAsync(ProductTypeCreateDTO dto);
        Task<ProductTypeReadDTO> UpdateAsync(int id, ProductTypeCreateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
