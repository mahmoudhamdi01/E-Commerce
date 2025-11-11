using AutoMapper;
using CoreLayer.Entities.OrderModule;
using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.MappingProfile
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDTO, OrderAddress>().ReverseMap();
            CreateMap<Order, OrderToReturnDTO>()
                   .ForMember(D => D.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName));

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(D => D.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                .ForMember(D => D.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<DeliveryMethod, DeliveryMethodDTO>().ReverseMap();

        }
    }
}
