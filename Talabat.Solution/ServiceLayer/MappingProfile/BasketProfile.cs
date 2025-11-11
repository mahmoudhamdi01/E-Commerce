using AutoMapper;
using CoreLayer.Entities.BasketModule;
using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.MappingProfile
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
