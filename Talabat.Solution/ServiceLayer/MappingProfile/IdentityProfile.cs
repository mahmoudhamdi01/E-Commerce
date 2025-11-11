using AutoMapper;
using CoreLayer.Entities.IdentityModule;
using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.MappingProfile
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile() 
        {
            CreateMap<Address, AddressDTO>().ReverseMap();
        }
    }
}
