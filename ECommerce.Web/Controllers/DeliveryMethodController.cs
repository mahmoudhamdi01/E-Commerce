using ECommerce.Interface.Interfaces;
using ECommerce.Interface.IServices.DeliveryMethod;
using ECommerce.Interface.Pagination;
using ECommerce.Web.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    public class DeliveryMethodController(IServiceManager _serviceManager) : ApiBaseController
    {
        [HttpGet("GetAll")]
        [Cache]
        public async Task<ActionResult<PagedResult<DeliveryMethodReadDTO>>>
            GetAll([FromQuery] BaseQueryParams queryParams)
        {
            var result = await _serviceManager.DeliveryMethodService.GetAllAsync(queryParams);
            return Ok(result);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<DeliveryMethodReadDTO>> GetById(int id)
        {
            var result = await _serviceManager.DeliveryMethodService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<DeliveryMethodReadDTO>>
            Create([FromBody] DeliveryMethodCreateDTO dto)
        {
            var result = await _serviceManager.DeliveryMethodService.AddAsync(dto);
            return Ok(result);
        }

        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult<DeliveryMethodReadDTO>>
            Update(int id, [FromBody] DeliveryMethodCreateDTO dto)
        {
            var result = await _serviceManager.DeliveryMethodService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _serviceManager.DeliveryMethodService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
