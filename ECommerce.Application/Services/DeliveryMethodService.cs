using AutoMapper;
using ECommerce.Application.Helpers;
using ECommerce.Infrastructure.Entities.OrderModule;
using ECommerce.Interface.Interfaces;
using ECommerce.Interface.IServices.DeliveryMethod;
using ECommerce.Interface.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class DeliveryMethodService(
    IUnitOfWork _unitOfWork,
    IMapper _mapper,
    IEntityAuditHelper _auditHelper
) : IDeliveryMethodService
    {
        private IGenericRepository<DeliveryMethod, int> Repo
            => _unitOfWork.GetRepository<DeliveryMethod, int>();

        public async Task<PagedResult<DeliveryMethodReadDTO>>
            GetAllAsync(BaseQueryParams queryParams)
        {
            var query = Repo.Query()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var search = queryParams.Search.Trim();
                query = query.Where(x =>
                    x.ShortName.Contains(search) ||
                    x.Description.Contains(search));
            }

            query = query.OrderByDescending(x => x.Id);

            return await query.ToPagedResultAsync(
                queryParams,
                entity => _mapper.Map<DeliveryMethodReadDTO>(entity)
            );
        }

        public async Task<DeliveryMethodReadDTO> GetByIdAsync(int id)
        {
            var entity = await Repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                throw new KeyNotFoundException($"Delivery method with id {id} not found.");

            return _mapper.Map<DeliveryMethodReadDTO>(entity);
        }

        public async Task<DeliveryMethodReadDTO> AddAsync(DeliveryMethodCreateDTO dto)
        {
            var entity = _mapper.Map<DeliveryMethod>(dto);
            _auditHelper.SetCreated(entity);

            await Repo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(entity.Id);
        }

        public async Task<DeliveryMethodReadDTO> UpdateAsync(int id, DeliveryMethodCreateDTO dto)
        {
            var entity = await Repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                throw new KeyNotFoundException($"Delivery method with id {id} not found.");

            entity.ShortName = dto.ShortName;
            entity.Description = dto.Description;
            entity.DeliveryTime = dto.DeliveryTime;
            entity.Price = dto.Price;
            _auditHelper.SetUpdated(entity);

            await _unitOfWork.SaveChangesAsync();
            return await GetByIdAsync(entity.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await Repo.GetByIdAsync(id);

            if (entity is null || entity.IsDeleted)
                return false;

            _auditHelper.SetSoftDeleted(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
