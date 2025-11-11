using Microsoft.AspNetCore.Mvc;
using ServiceAbstractionLayer;
using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    public class BasketController(IServiceManager _serviceManager) : APIBaseController
    {
        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasket(string Key)
        {
            var Basket = await _serviceManager.BasketService.GetBasketAsync(Key);
            return Ok(Basket);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket(BasketDTO basket)
        {
            var Basket = await _serviceManager.BasketService.CreateOrUpdateBasket(basket);
            return Ok(Basket);
        }

        [HttpDelete("{Key}")]
        public async Task<ActionResult<bool>> DeleteBasket(string Key)
        {
            var Result = await _serviceManager.BasketService.DeleteBasketAsync(Key);
            return Ok(Result);
        }
    }
}
