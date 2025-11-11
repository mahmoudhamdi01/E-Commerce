using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Attributes;
using ServiceAbstractionLayer;
using SharedLayer;
using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    public class ProductController(IServiceManager _serviceManager) : APIBaseController
    {
        [Authorize]
        [HttpGet("Products")]
        [Cache]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery] ProductQueryParams queryParams)
        {
            var Products = await _serviceManager.ProductService.GetAllProductsAsync(queryParams);
            return Ok(Products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var Product = await _serviceManager.ProductService.GetProductByIdAsync(id);
            return Ok(Product);
        }

        [HttpGet("Brands")]
        [Cache]
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrands()
        {
            var Brands = await _serviceManager.ProductService.GetAllBrandsAsync();
            return Ok(Brands);
        }

        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllTypes()
        {
            var Types = await _serviceManager.ProductService.GetAllTypesAsync();
            return Ok(Types);
        }
    }
}
