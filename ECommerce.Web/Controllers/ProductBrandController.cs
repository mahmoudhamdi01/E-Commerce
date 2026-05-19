using ECommerce.Interface.Interfaces;
using ECommerce.Interface.IServices.ProductBrand;
using ECommerce.Interface.Pagination;
using ECommerce.Web.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    public class ProductBrandController(IServiceManager _serviceManager) : ApiBaseController
    {
        [HttpGet("GetAll")]
        [Cache]
        public async Task<ActionResult<PagedResult<ProductBrandReadDTO>>> GetAll([FromQuery] ProductBrandQueryParams queryParams)
        {
            var result = await _serviceManager.ProductBrandService.GetAllAsync(queryParams);
            return Ok(result);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<ProductBrandReadDTO>> GetById(int id)
        {
            var result = await _serviceManager.ProductBrandService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ProductBrandReadDTO>> Create([FromBody] ProductBrandCreateDTO dto)
        {
            var result = await _serviceManager.ProductBrandService.AddAsync(dto);
            return Ok(result);
        }

        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult<ProductBrandReadDTO>> Update(int id, [FromBody] ProductBrandCreateDTO dto)
        {
            var result = await _serviceManager.ProductBrandService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _serviceManager.ProductBrandService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
