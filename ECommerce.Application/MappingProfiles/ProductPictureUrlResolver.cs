using AutoMapper;
using ECommerce.Infrastructure.Entities.Products;
using ECommerce.Interface.IServices.Product;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.MappingProfiles
{
    public class ProductPictureUrlResolver(IConfiguration configuration) : IValueResolver<Product, ProductReadDTO, string>
    {
        public string Resolve(Product source, ProductReadDTO destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PictureUrl))
                return string.Empty;
            else
                return $"{configuration["BaseUrl"]}{source.PictureUrl}";
        }
    }
}
