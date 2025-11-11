using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstractionLayer;
using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    public class OrderController(IServiceManager _serviceManager) : APIBaseController
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDTO)
        {
            var Order = await _serviceManager.OrderService.CreateOrder(orderDTO, GetEmailForToken());
            return Ok(Order);
        }

        [HttpGet("Orders")]
        public async Task<ActionResult<IEnumerable<OrderToReturnDTO>>> GetAllOrders(string Email)
        {
            var Orders = await _serviceManager.OrderService.GetAllOrdersAsync(Email);
            return Ok(Orders);
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetAllDeliveryMethod()
        {
            var Result = await _serviceManager.OrderService.GetDeliveryMethodsAsync();
            return Ok(Result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrder(Guid Id)
        {
            var Order = await _serviceManager.OrderService.GetOrderByIdAsync(Id);
            return Ok(Order);
        }
    }
}
