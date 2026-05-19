using AutoMapper;
using ECommerce.Infrastructure.Entities.Products;
using ECommerce.Interface.IServices.ProductType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.MappingProfiles
{
    public class ProductTypeProfile : Profile
    {
        public ProductTypeProfile()
        {
            CreateMap<ProductTypeCreateDTO, ProductType>();

            CreateMap<ProductType, ProductTypeReadDTO>()
                .ForMember(d => d.Title, o => o.Ignore())
                .ForMember(d => d.Description, o => o.Ignore());
        }
    }
}
