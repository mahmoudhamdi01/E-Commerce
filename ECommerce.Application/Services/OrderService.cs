using AutoMapper;
using ECommerce.Application.Helpers;
using ECommerce.Infrastructure.Entities.OrderModule;
using ECommerce.Infrastructure.Entities.Products;
using ECommerce.Infrastructure.Exceptions;
using ECommerce.Interface.Interfaces;
using ECommerce.Interface.IServices.Authentication;
using ECommerce.Interface.IServices.Order;
using ECommerce.Interface.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class OrderService(IMapper _mapper, IBasketRepository _basketRepo,
        IUnitOfWork _unitOfWork, IEntityAuditHelper _auditHelper) : IOrderService
    {
        public async Task<OrderToReturnDTO> CreateOrder(OrderDTO orderDTO, string Email)
        {
            var OrderAddress = _mapper.Map<AddressDTO, OrderAddress>(orderDTO.Address);
            var Basket = await _basketRepo.GetBasketAsync(orderDTO.BasketId)
                ?? throw new BasketNotFoundException(orderDTO.BasketId);

            List<OrderItem> OrderItems = [];
            var ProductRepo = _unitOfWork.GetRepository<Product, int>();

            foreach (var item in Basket.Items)
            {
                var Product = await ProductRepo.GetByIdAsync(item.Id)
                    ?? throw new ProductNotFoundException(item.Id);

                var orderItem = new OrderItem()
                {
                    Product = new ProductItemOrdered()
                    {
                        ProductId = Product.Id,
                        PictureUrl = Product.PictureUrl,
                        ProductName = Product.TitleEnglish
                    },
                    Price = Product.Price,
                    Quantity = item.Quantity
                };
                _auditHelper.SetCreated(orderItem);
                OrderItems.Add(orderItem);
            }

            var DeliveryMethod = await _unitOfWork
                .GetRepository<DeliveryMethod, int>()
                .GetByIdAsync(orderDTO.DeliveryMethodId);

            var SubTotal = OrderItems.Sum(I => I.Quantity * I.Price);
            var Order = new Order(Email, OrderAddress, DeliveryMethod, OrderItems, SubTotal);
            _auditHelper.SetCreated(Order);
            await _unitOfWork.GetRepository<Order, Guid>().AddAsync(Order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<Order, OrderToReturnDTO>(Order);
        }

        public async Task<PagedResult<OrderToReturnDTO>>
            GetAllOrdersAsync(string Email, BaseQueryParams queryParams)
        {
            var query = _unitOfWork
                .GetRepository<Order, Guid>()
                .Query()
                .Include(O => O.Items)
                .Include(O => O.DeliveryMethod)
                .Where(O => O.UserEmail == Email);

            return await query.ToPagedResultAsync(
                queryParams,
                entity => _mapper.Map<OrderToReturnDTO>(entity)
            );
        }

        public async Task<PagedResult<DeliveryMethodDTO>>
       GetDeliveryMethodsAsync(BaseQueryParams queryParams)
        {
            var query = _unitOfWork
                .GetRepository<DeliveryMethod, int>()
                .Query();

            return await query.ToPagedResultAsync(
                queryParams,
                entity => _mapper.Map<DeliveryMethodDTO>(entity)
            );
        }

        public async Task<OrderToReturnDTO> GetOrderByIdAsync(Guid Id)
        {
            var order = await _unitOfWork
                .GetRepository<Order, Guid>()
                .Query()
                .Include(O => O.Items)
                .Include(O => O.DeliveryMethod)
                .FirstOrDefaultAsync(O => O.Id == Id)
                ?? throw new OrderNotFoundException(Id);

            return _mapper.Map<OrderToReturnDTO>(order);
        }
    }
}
