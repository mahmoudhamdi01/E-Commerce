using AutoMapper;
using CoreLayer.Entities;
using CoreLayer.Exceptions;
using CoreLayer.Interfaces;
using ServiceAbstractionLayer;
using ServiceLayer.Specifications;
using SharedLayer;
using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
        {
            var Brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var MappedBrands = _mapper.Map<IEnumerable<ProductBrand>, IEnumerable<BrandDTO>>(Brands);
            return MappedBrands;
        }

        public async Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var Spec = new ProductWithBrandAndTypeSpec(queryParams);
            var Repo = _unitOfWork.GetRepository<Product, int>();
            var Products = await Repo.GetAllAsync(Spec);
            var MappedProducts = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(Products);
            var ProductCount = MappedProducts.Count();
            var TotalCount = await Repo.CountAsync(new ProductCountSpec(queryParams));
            return new PaginatedResult<ProductDTO>(queryParams.PageIndex, ProductCount, TotalCount, MappedProducts);
        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
        {
            var Types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var MappedTypes = _mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDTO>>(Types);
            return MappedTypes;
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpec(id);
            var Product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(Spec);
            if(Product is null)
                throw new ProductNotFoundException(id);
            var MappedProduct = _mapper.Map<Product, ProductDTO>(Product);
            return MappedProduct;
        }
    }
}
