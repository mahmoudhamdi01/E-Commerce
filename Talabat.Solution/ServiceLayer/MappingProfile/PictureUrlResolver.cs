using AutoMapper;
using CoreLayer.Entities;
using Microsoft.Extensions.Configuration;
using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.MappingProfile
{
    public class PictureUrlResolver(IConfiguration _configuration) : IValueResolver<Product, ProductDTO, string>
    {
        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configuration["BaseUrl"]}{source.PictureUrl}";
            return string.Empty;
        }
    }
}
