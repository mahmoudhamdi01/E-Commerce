using ECommerce.Interface.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Interface.IServices.Order
{
    public interface IOrderService
    {
        Task<OrderToReturnDTO> CreateOrder(OrderDTO orderDTO, string Email);
        Task<PagedResult<DeliveryMethodDTO>> GetDeliveryMethodsAsync(BaseQueryParams queryParams);
        Task<PagedResult<OrderToReturnDTO>> GetAllOrdersAsync(string Email, BaseQueryParams queryParams);
        Task<OrderToReturnDTO> GetOrderByIdAsync(Guid Id);
    }
}
