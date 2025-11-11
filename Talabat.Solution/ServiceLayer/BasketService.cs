using AutoMapper;
using CoreLayer.Entities.BasketModule;
using CoreLayer.Exceptions;
using CoreLayer.Interfaces;
using ServiceAbstractionLayer;
using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class BasketService(IBasketRepository _basketRepo, IMapper _mapper) : IBasketService
    {
        public async Task<BasketDTO> CreateOrUpdateBasket(BasketDTO basket)
        {
            var CustomerBasket = _mapper.Map<BasketDTO, CustomerBasket>(basket);
            var CreatedOrUpdated = await _basketRepo.CreateOrUpdateBasket(CustomerBasket);
            if (CreatedOrUpdated is not null)
                return await GetBasketAsync(basket.Id);
            else
                throw new Exception("Can Not Create Or Delete Basket Now, Try Again Later");
        }

        public async Task<bool> DeleteBasketAsync(string Key)
        {
            return await _basketRepo.DeleteBasketAsync(Key);
        }

        public async Task<BasketDTO> GetBasketAsync(string Key)
        {
            var Basket = await _basketRepo.GetBasketAsync(Key);
            if (Basket is null)
                throw new BasketNotFoundException(Key);
            else
                return _mapper.Map<CustomerBasket, BasketDTO>(Basket);
        }
    }
}
