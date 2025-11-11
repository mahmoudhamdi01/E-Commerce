using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstractionLayer
{
    public interface IBasketService
    {
        public Task<BasketDTO> GetBasketAsync(string Key);
        public Task<BasketDTO> CreateOrUpdateBasket(BasketDTO basket);
        Task<bool> DeleteBasketAsync(string Key);
    }
}
