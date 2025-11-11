using CoreLayer.Entities.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Interfaces
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string Key);
        Task<CustomerBasket?> CreateOrUpdateBasket(CustomerBasket basket, TimeSpan? TimeToLive = null);
        Task<bool> DeleteBasketAsync(string Id);
    }
}
