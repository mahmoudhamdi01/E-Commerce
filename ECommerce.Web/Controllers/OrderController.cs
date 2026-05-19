using ECommerce.Interface.Interfaces;
using ECommerce.Interface.IServices.Order;
using ECommerce.Interface.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    public class OrderController(IServiceManager _serviceManager) : ApiBaseController
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDTO)
        {
            var Order = await _serviceManager.OrderService.CreateOrder(orderDTO, GetEmailForToken());
            return Ok(Order);
        }

        [Authorize]
        [HttpGet("Orders")]
        public async Task<ActionResult<PagedResult<OrderToReturnDTO>>> GetAllOrders([FromQuery] BaseQueryParams queryParams)
        {
            var Email = GetEmailForToken();
            var Orders = await _serviceManager.OrderService.GetAllOrdersAsync(Email, queryParams);
            return Ok(Orders);
        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<PagedResult<DeliveryMethodDTO>>> GetAllDeliveryMethod([FromQuery] BaseQueryParams queryParams)
        {
            var Result = await _serviceManager.OrderService.GetDeliveryMethodsAsync(queryParams);
            return Ok(Result);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrder(Guid Id)
        {
            var Order = await _serviceManager.OrderService.GetOrderByIdAsync(Id);
            return Ok(Order);
        }
    }
}
