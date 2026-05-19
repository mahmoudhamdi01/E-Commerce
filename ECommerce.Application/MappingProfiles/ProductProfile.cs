using AutoMapper;
using ECommerce.Infrastructure.Entities.Products;
using ECommerce.Interface.IServices.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductCreateUpdateDTO, Product>()
                .ForMember(d => d.PictureUrl, o => o.Ignore());

            CreateMap<Product, ProductReadDTO>()
                .ForMember(d => d.Title, o => o.Ignore())
                .ForMember(d => d.Description, o => o.Ignore())
                .ForMember(d => d.BrandName, o => o.Ignore())
                .ForMember(d => d.TypeName, o => o.Ignore())
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());
        }
    }
}
 