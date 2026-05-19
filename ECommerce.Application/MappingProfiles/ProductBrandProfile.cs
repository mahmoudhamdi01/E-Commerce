using AutoMapper;
using ECommerce.Infrastructure.Entities.Products;
using ECommerce.Interface.IServices.ProductBrand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.MappingProfiles
{
    public class ProductBrandProfile : Profile
    {
        public ProductBrandProfile()
        {
            CreateMap<ProductBrandCreateDTO, ProductBrand>();

            CreateMap<ProductBrand, ProductBrandReadDTO>()
                .ForMember(d => d.Title, o => o.Ignore())
                .ForMember(d => d.Description, o => o.Ignore());
        }
    }
}
