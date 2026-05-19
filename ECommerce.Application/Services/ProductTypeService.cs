using AutoMapper;
using ECommerce.Application.Helpers;
using ECommerce.Infrastructure.Entities.Products;
using ECommerce.Interface.Interfaces;
using ECommerce.Interface.IServices.ProductType;
using ECommerce.Interface.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILocalizationService _localizationService;
        private readonly IEntityAuditHelper _entityAuditHelper;

        public ProductTypeService(
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

        public async Task<PagedResult<ProductTypeReadDTO>> GetAllAsync(ProductTypeQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<ProductType, int>();

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
                var dto = _mapper.Map<ProductTypeReadDTO>(entity);
                dto.Title = _localizationService.GetLocalizedTitle(entity);
                dto.Description = _localizationService.GetLocalizedDescription(entity);
                return dto;
            });
        }

        public async Task<ProductTypeReadDTO> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<ProductType, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                throw new KeyNotFoundException("Product type not found.");

            var dto = _mapper.Map<ProductTypeReadDTO>(entity);
            dto.Title = _localizationService.GetLocalizedTitle(entity);
            dto.Description = _localizationService.GetLocalizedDescription(entity);

            return dto;
        }

        public async Task<ProductTypeReadDTO> AddAsync(ProductTypeCreateDTO dto)
        {
            var repo = _unitOfWork.GetRepository<ProductType, int>();

            var entity = _mapper.Map<ProductType>(dto);
            _entityAuditHelper.SetCreated(entity);

            await repo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(entity.Id);
        }

        public async Task<ProductTypeReadDTO> UpdateAsync(int id, ProductTypeCreateDTO dto)
        {
            var repo = _unitOfWork.GetRepository<ProductType, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                throw new KeyNotFoundException("Product type not found.");

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
            var repo = _unitOfWork.GetRepository<ProductType, int>();
            var entity = await repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                return false;

            _entityAuditHelper.SetSoftDeleted(entity);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
