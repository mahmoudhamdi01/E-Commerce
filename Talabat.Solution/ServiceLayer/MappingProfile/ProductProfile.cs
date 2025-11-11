using AutoMapper;
using CoreLayer.Entities;
using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.MappingProfile
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTO>()
                   .ForMember(S => S.BrandName, O => O.MapFrom(D => D.ProductBrand.Name))
                   .ForMember(S => S.TypeName, O => O.MapFrom(D => D.ProductType.Name))
                   .ForMember(S => S.PictureUrl, O => O.MapFrom<PictureUrlResolver>());

            CreateMap<ProductBrand, BrandDTO>();
            CreateMap<ProductType, TypeDTO>();
        }
    }
}
