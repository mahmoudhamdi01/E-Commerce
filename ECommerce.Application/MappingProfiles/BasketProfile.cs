using AutoMapper;
using ECommerce.Infrastructure.Entities.BasketModule;
using ECommerce.Interface.IServices.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.MappingProfiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket, BasketDTO>().ReverseMap();
            CreateMap<BasketItems, BasketItemDTO>().ReverseMap();
        }
    }
}
