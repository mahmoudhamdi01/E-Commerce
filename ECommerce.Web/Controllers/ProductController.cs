using ECommerce.Interface.Interfaces;
using ECommerce.Interface.IServices.Product;
using ECommerce.Interface.Pagination;
using ECommerce.Web.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    public class ProductController(IServiceManager _serviceManager) : ApiBaseController
    {
        [HttpGet("GetAll")]
        [Cache]
        public async Task<ActionResult<PagedResult<ProductReadDTO>>> GetAll([FromQuery] ProductQueryParams queryParams)
        {
            var products = await _serviceManager.ProductService.GetAllProductsAsync(queryParams);
            return Ok(products);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<ProductReadDTO>> GetById(int id)
        {
            var product = await _serviceManager.ProductService.GetProductById(id);
            return Ok(product);
        }

        [HttpGet("GetByBrandId/{brandId:int}")]
        [Cache]
        public async Task<ActionResult<PagedResult<ProductReadDTO>>> GetByBrandId(int brandId, [FromQuery] ProductQueryParams queryParams)
        {
            var products = await _serviceManager.ProductService.GetProductsByBrandId(brandId, queryParams);
            return Ok(products);
        }

        [HttpPost("Create")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductReadDTO>> Create([FromForm] ProductCreateUpdateDTO dto)
        {
            var product = await _serviceManager.ProductService.AddProductAsync(dto);
            return Ok(product);
        }

        [HttpPut("Update/{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductReadDTO>> Update(int id, [FromForm] ProductCreateUpdateDTO dto)
        {
            var product = await _serviceManager.ProductService.UpdateProductAsync(id, dto);
            return Ok(product);
        } 

        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _serviceManager.ProductService.DeleteProductAsync(id);
            return Ok(result);
        }
    }
}
