using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstractionLayer
{
    public interface IOrderService
    {
        Task<OrderToReturnDTO> CreateOrder(OrderDTO orderDTO, string Email);
        Task<IEnumerable<DeliveryMethodDTO>> GetDeliveryMethodsAsync();
        Task<IEnumerable<OrderToReturnDTO>> GetAllOrdersAsync(string Email);
        Task<OrderToReturnDTO> GetOrderByIdAsync(Guid Id);
    }
}
