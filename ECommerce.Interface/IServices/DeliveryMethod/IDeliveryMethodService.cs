using ECommerce.Interface.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Interface.IServices.DeliveryMethod
{
    public interface IDeliveryMethodService
    {
        Task<PagedResult<DeliveryMethodReadDTO>> GetAllAsync(BaseQueryParams queryParams);
        Task<DeliveryMethodReadDTO> GetByIdAsync(int id);
        Task<DeliveryMethodReadDTO> AddAsync(DeliveryMethodCreateDTO dto);
        Task<DeliveryMethodReadDTO> UpdateAsync(int id, DeliveryMethodCreateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
