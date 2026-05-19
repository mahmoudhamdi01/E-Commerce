using AutoMapper;
using ECommerce.Infrastructure.Entities.OrderModule;
using ECommerce.Interface.IServices.DeliveryMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.MappingProfiles
{
    public class DeliveryMethodProfile : Profile
    {
        public DeliveryMethodProfile()
        {
            CreateMap<DeliveryMethodCreateDTO, DeliveryMethod>();

            CreateMap<DeliveryMethod, DeliveryMethodReadDTO>();
        }
    }
}
