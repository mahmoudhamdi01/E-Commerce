using AutoMapper;
using CoreLayer.Entities;
using CoreLayer.Entities.OrderModule;
using CoreLayer.Exceptions;
using CoreLayer.Interfaces;
using ServiceAbstractionLayer;
using ServiceLayer.Specifications;
using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class OrderService(IMapper _mapper, IBasketRepository _basketRepo, IUnitOfWork _unitOfWork) : IOrderService
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
                    Product = new ProductItemOrdered() { ProductId = Product.Id, PictureUrl = Product.PictureUrl, ProductName = Product.Name },
                    Price = Product.Price,
                    Quantity = item.Quantity
                };
                OrderItems.Add(orderItem);
            }

            var DeliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDTO.DeliveryMethodId);

            var SubTotal = OrderItems.Sum(I => I.Quantity * I.Price);

            var Order = new Order(Email, OrderAddress, DeliveryMethod, OrderItems, SubTotal);

            await _unitOfWork.GetRepository<Order, Guid>().AddAsync(Order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<Order, OrderToReturnDTO>(Order);
        }

        public async Task<IEnumerable<OrderToReturnDTO>> GetAllOrdersAsync(string Email)
        {
            var Spec = new OrderSpecification(Email);
            var Orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(Spec);
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderToReturnDTO>>(Orders);
        }

        public async Task<IEnumerable<DeliveryMethodDTO>> GetDeliveryMethodsAsync()
        {
            var DeliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMethodDTO>>(DeliveryMethods);
        }

        public async Task<OrderToReturnDTO> GetOrderByIdAsync(Guid Id)
        {
            var Spec = new OrderSpecification(Id);
            var Order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(Spec);
            return _mapper.Map<Order, OrderToReturnDTO>(Order);
        }
    }
}
