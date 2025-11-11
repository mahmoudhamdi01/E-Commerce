using AutoMapper;
using AutoMapper.Execution;
using CoreLayer.Entities.OrderModule;
using Microsoft.Extensions.Configuration;
using SharedLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.MappingProfile
{
    public class OrderItemPictureUrlResolver(IConfiguration _configuration) : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
                return $"{_configuration["BaseUrl"]}{source.Product.PictureUrl}";
            return string.Empty;
        }
    }
}
