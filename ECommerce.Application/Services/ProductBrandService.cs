using AutoMapper;
using ECommerce.Application.Helpers;
using ECommerce.Infrastructure.Entities.Products;
using ECommerce.Interface.Interfaces;
using ECommerce.Interface.IServices.ProductBrand;
using ECommerce.Interface.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class ProductBrandService : IProductBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IEntityAuditHelper _entityAuditHelper;

        public ProductBrandService(
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

        public async Task<PagedResult<ProductBrandReadDTO>> GetAllAsync(ProductBrandQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<ProductBrand, int>();

            var query = repo.Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var search = queryParams.Search.Trim();

                query = query.Where(x =>
                    x.TitleArabic.Contains(search) ||
                    x.TitleEnglish.Contains(search));
            }

            query = query.OrderByDescending(x => x.Id);

            return await query.ToPagedResultAsync(queryParams, entity =>
            {
                var dto = _mapper.Map<ProductBrandReadDTO>(entity);
                dto.Title = _localizationService.GetLocalizedTitle(entity);
                dto.Description = _localizationService.GetLocalizedDescription(entity);
                return dto;
            });
        }

        public async Task<ProductBrandReadDTO> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<ProductBrand, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                throw new KeyNotFoundException("Product brand not found.");

            var dto = _mapper.Map<ProductBrandReadDTO>(entity);
            dto.Title = _localizationService.GetLocalizedTitle(entity);
            dto.Description = _localizationService.GetLocalizedDescription(entity);

            return dto;
        }

        public async Task<ProductBrandReadDTO> AddAsync(ProductBrandCreateDTO dto)
        {
            var repo = _unitOfWork.GetRepository<ProductBrand, int>();

            var entity = _mapper.Map<ProductBrand>(dto);
            _entityAuditHelper.SetCreated(entity);

            await repo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(entity.Id);
        }

        public async Task<ProductBrandReadDTO> UpdateAsync(int id, ProductBrandCreateDTO dto)
        {
            var repo = _unitOfWork.GetRepository<ProductBrand, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                throw new KeyNotFoundException("Product brand not found.");

            entity.TitleArabic = dto.TitleArabic;
            entity.TitleEnglish = dto.TitleEnglish;
            entity.DescriptionAr = dto.DescriptionAr;
            entity.DescriptionEn = dto.DescriptionEn;

            _entityAuditHelper.SetUpdated(entity);

            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(entity.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<ProductBrand, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                return false;

            _entityAuditHelper.SetSoftDeleted(entity);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
