using ECommerce.Interface.Interfaces;
using ECommerce.Interface.IServices.ProductType;
using ECommerce.Interface.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    public class ProductTypeController(IServiceManager _serviceManager) : ApiBaseController
    {
        [HttpGet("GetAll")]
        public async Task<ActionResult<PagedResult<ProductTypeReadDTO>>> GetAll([FromQuery] ProductTypeQueryParams queryParams)
        {
            var result = await _serviceManager.ProductTypeService.GetAllAsync(queryParams);
            return Ok(result);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<ProductTypeReadDTO>> GetById(int id)
        {
            var result = await _serviceManager.ProductTypeService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ProductTypeReadDTO>> Create([FromBody] ProductTypeCreateDTO dto)
        {
            var result = await _serviceManager.ProductTypeService.AddAsync(dto);
            return Ok(result);
        }

        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult<ProductTypeReadDTO>> Update(int id, [FromBody] ProductTypeCreateDTO dto)
        {
            var result = await _serviceManager.ProductTypeService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _serviceManager.ProductTypeService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
