using AutoMapper;
using ECommerce.Infrastructure.Entities.IdentityModule;
using ECommerce.Interface.IServices.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.MappingProfiles
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            CreateMap<Address, AddressDTO>().ReverseMap();
        }
    }
}
