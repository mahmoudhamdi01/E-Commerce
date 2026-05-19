using AutoMapper;
using ECommerce.Application.Helpers;
using ECommerce.Infrastructure.Entities.Products;
using ECommerce.Interface.Interfaces;
using ECommerce.Interface.IServices.Product;
using ECommerce.Interface.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IEntityAuditHelper _entityAuditHelper;

        public ProductService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILocalizationService localizationService,
            IEntityAuditHelper entityAuditHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizationService = localizationService;
            _entityAuditHelper = entityAuditHelper;
        }

        //public async Task<IEnumerable<ProductReadDTO>> GetAllProductsAsync()
        //{
        //    var productRepo = _unitOfWork.GetRepository<Product, int>();
        //    var brandRepo = _unitOfWork.GetRepository<ProductBrand, int>();
        //    var typeRepo = _unitOfWork.GetRepository<ProductType, int>();

        //    var products = (await productRepo.GetAllAsync()).ToList();
        //    var brands = (await brandRepo.GetAllAsync()).ToDictionary(x => x.Id, x => x);
        //    var types = (await typeRepo.GetAllAsync()).ToDictionary(x => x.Id, x => x);

        //    var result = products.Select(product =>
        //    {
        //        var dto = _mapper.Map<ProductReadDTO>(product);

        //        dto.Title = _localizationService.GetLocalizedTitle(product);
        //        dto.Description = _localizationService.GetLocalizedDescription(product);

        //        if (brands.TryGetValue(product.BrandId, out var brand))
        //            dto.BrandName = _localizationService.GetLocalizedTitle(brand);

        //        if (types.TryGetValue(product.TypeId, out var type))
        //            dto.TypeName = _localizationService.GetLocalizedTitle(type);

        //        return dto;
        //    }).ToList();

        //    return result;
        //}

        public async Task<PagedResult<ProductReadDTO>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var productRepo = _unitOfWork.GetRepository<Product, int>();
            var brandRepo = _unitOfWork.GetRepository<ProductBrand, int>();
            var typeRepo = _unitOfWork.GetRepository<ProductType, int>();

            var query = productRepo.Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var search = queryParams.Search.Trim();

                query = query.Where(x =>
                    x.TitleArabic.Contains(search) ||
                    x.TitleEnglish.Contains(search));
            }

            if (queryParams.BrandId.HasValue)
                query = query.Where(x => x.BrandId == queryParams.BrandId.Value);

            if (queryParams.TypeId.HasValue)
                query = query.Where(x => x.TypeId == queryParams.TypeId.Value);

            query = ApplyProductSorting(query, queryParams);

            var brands = await brandRepo.Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .ToDictionaryAsync(x => x.Id, x => x);

            var types = await typeRepo.Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .ToDictionaryAsync(x => x.Id, x => x);

            return await query.ToPagedResultAsync(queryParams, product =>
            {
                var dto = _mapper.Map<ProductReadDTO>(product);

                dto.Title = _localizationService.GetLocalizedTitle(product);
                dto.Description = _localizationService.GetLocalizedDescription(product);

                if (brands.TryGetValue(product.BrandId, out var brand))
                    dto.BrandName = _localizationService.GetLocalizedTitle(brand);

                if (types.TryGetValue(product.TypeId, out var type))
                    dto.TypeName = _localizationService.GetLocalizedTitle(type);

                return dto;
            });
        }

        public async Task<ProductReadDTO> GetProductById(int id)
        {
            var productRepo = _unitOfWork.GetRepository<Product, int>();
            var brandRepo = _unitOfWork.GetRepository<ProductBrand, int>();
            var typeRepo = _unitOfWork.GetRepository<ProductType, int>();

            var product = await productRepo.GetByIdAsync(id);

            if (product is null || product.IsDeleted)
                throw new KeyNotFoundException("Product not found.");

            var dto = _mapper.Map<ProductReadDTO>(product);

            dto.Title = _localizationService.GetLocalizedTitle(product);
            dto.Description = _localizationService.GetLocalizedDescription(product);

            var brand = await brandRepo.GetByIdAsync(product.BrandId);
            var type = await typeRepo.GetByIdAsync(product.TypeId);

            if (brand is not null && !brand.IsDeleted)
                dto.BrandName = _localizationService.GetLocalizedTitle(brand);

            if (type is not null && !type.IsDeleted)
                dto.TypeName = _localizationService.GetLocalizedTitle(type);

            return dto;
        }

        public async Task<ProductReadDTO> AddProductAsync(ProductCreateUpdateDTO productCreateUpdateDTO)
        {
            await ValidateBrandAndType(productCreateUpdateDTO.BrandId, productCreateUpdateDTO.TypeId);

            var productRepo = _unitOfWork.GetRepository<Product, int>();

            var product = _mapper.Map<Product>(productCreateUpdateDTO);

            if (productCreateUpdateDTO.Picture is not null)
            {
                product.PictureUrl = DocumentSettings.UploadFile(productCreateUpdateDTO.Picture, "products");
            }

            _entityAuditHelper.SetCreated(product);

            await productRepo.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return await GetProductById(product.Id);
        }

        public async Task<ProductReadDTO> UpdateProductAsync(int id, ProductCreateUpdateDTO productCreateUpdateDTO)
        {
            await ValidateBrandAndType(productCreateUpdateDTO.BrandId, productCreateUpdateDTO.TypeId);

            var productRepo = _unitOfWork.GetRepository<Product, int>();
            var product = await productRepo.GetByIdAsync(id);

            if (product is null || product.IsDeleted)
                throw new KeyNotFoundException("Product not found.");

            var oldPicture = product.PictureUrl;
            var newPictureUploaded = false;

            product.TitleArabic = productCreateUpdateDTO.TitleArabic;
            product.TitleEnglish = productCreateUpdateDTO.TitleEnglish;
            product.DescriptionAr = productCreateUpdateDTO.DescriptionAr;
            product.DescriptionEn = productCreateUpdateDTO.DescriptionEn;
            product.Price = productCreateUpdateDTO.Price;
            product.BrandId = productCreateUpdateDTO.BrandId;
            product.TypeId = productCreateUpdateDTO.TypeId;

            if (productCreateUpdateDTO.Picture is not null)
            {
                product.PictureUrl = DocumentSettings.UploadFile(productCreateUpdateDTO.Picture, "products");
                newPictureUploaded = true;
            }

            _entityAuditHelper.SetUpdated(product);

            await _unitOfWork.SaveChangesAsync();

            if (newPictureUploaded && !string.IsNullOrWhiteSpace(oldPicture))
            {
                DocumentSettings.DeleteFile(oldPicture, "products");
            }

            return await GetProductById(product.Id);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var productRepo = _unitOfWork.GetRepository<Product, int>();
            var product = await productRepo.GetByIdAsync(id);

            if (product is null || product.IsDeleted)
                return false;

            _entityAuditHelper.SetSoftDeleted(product);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        //public async Task<IEnumerable<ProductReadDTO>> GetProductsByBrandId(int brandId)
        //{
        //    var productRepo = _unitOfWork.GetRepository<Product, int>();
        //    var brandRepo = _unitOfWork.GetRepository<ProductBrand, int>();
        //    var typeRepo = _unitOfWork.GetRepository<ProductType, int>();

        //    var products = (await productRepo.GetAllAsync())
        //        .Where(x => x.BrandId == brandId && !x.IsDeleted)
        //        .ToList();

        //    var brands = (await brandRepo.GetAllAsync()).ToDictionary(x => x.Id, x => x);
        //    var types = (await typeRepo.GetAllAsync()).ToDictionary(x => x.Id, x => x);

        //    var result = products.Select(product =>
        //    {
        //        var dto = _mapper.Map<ProductReadDTO>(product);

        //        dto.Title = _localizationService.GetLocalizedTitle(product);
        //        dto.Description = _localizationService.GetLocalizedDescription(product);

        //        if (brands.TryGetValue(product.BrandId, out var brand))
        //            dto.BrandName = _localizationService.GetLocalizedTitle(brand);

        //        if (types.TryGetValue(product.TypeId, out var type))
        //            dto.TypeName = _localizationService.GetLocalizedTitle(type);

        //        return dto;
        //    }).ToList();

        //    return result;
        //}

        public async Task<PagedResult<ProductReadDTO>> GetProductsByBrandId(int brandId, ProductQueryParams queryParams)
        {
            var productRepo = _unitOfWork.GetRepository<Product, int>();
            var brandRepo = _unitOfWork.GetRepository<ProductBrand, int>();
            var typeRepo = _unitOfWork.GetRepository<ProductType, int>();

            var query = productRepo.Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.BrandId == brandId);

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var search = queryParams.Search.Trim();

                query = query.Where(x =>
                    x.TitleArabic.Contains(search) ||
                    x.TitleEnglish.Contains(search));
            }

            if (queryParams.TypeId.HasValue)
                query = query.Where(x => x.TypeId == queryParams.TypeId.Value);

            query = ApplyProductSorting(query, queryParams);

            var brands = await brandRepo.Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .ToDictionaryAsync(x => x.Id, x => x);

            var types = await typeRepo.Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .ToDictionaryAsync(x => x.Id, x => x);

            return await query.ToPagedResultAsync(queryParams, product =>
            {
                var dto = _mapper.Map<ProductReadDTO>(product);

                dto.Title = _localizationService.GetLocalizedTitle(product);
                dto.Description = _localizationService.GetLocalizedDescription(product);

                if (brands.TryGetValue(product.BrandId, out var brand))
                    dto.BrandName = _localizationService.GetLocalizedTitle(brand);

                if (types.TryGetValue(product.TypeId, out var type))
                    dto.TypeName = _localizationService.GetLocalizedTitle(type);

                return dto;
            });
        }

        private async Task ValidateBrandAndType(int brandId, int typeId)
        {
            var brandRepo = _unitOfWork.GetRepository<ProductBrand, int>();
            var typeRepo = _unitOfWork.GetRepository<ProductType, int>();

            var brand = await brandRepo.GetByIdAsync(brandId);
            if (brand is null || brand.IsDeleted)
                throw new KeyNotFoundException("Brand not found.");

            var type = await typeRepo.GetByIdAsync(typeId);
            if (type is null || type.IsDeleted)
                throw new KeyNotFoundException("Type not found.");
        }

        private IQueryable<Product> ApplyProductSorting(IQueryable<Product> query, ProductQueryParams queryParams)
        {
            var sortBy = queryParams.SortBy?.Trim().ToLower();
            var isDesc = queryParams.SortDirection?.Trim().ToLower() == "desc";

            return sortBy switch
            {
                "price" => isDesc ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price),
                "title" => isDesc ? query.OrderByDescending(x => x.TitleEnglish) : query.OrderBy(x => x.TitleEnglish),
                _ => query.OrderByDescending(x => x.Id)
            };
        }

    }
}
