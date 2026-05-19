using AutoMapper;
using ECommerce.Application.Services;
using ECommerce.Infrastructure.Entities.IdentityModule;
using ECommerce.Interface.Interfaces;
using ECommerce.Interface.IServices.Authentication;
using ECommerce.Interface.IServices.Basket;
using ECommerce.Interface.IServices.DeliveryMethod;
using ECommerce.Interface.IServices.Order;
using ECommerce.Interface.IServices.Product;
using ECommerce.Interface.IServices.ProductBrand;
using ECommerce.Interface.IServices.ProductType;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories
{
    public class ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IEntityAuditHelper entityAuditHelper,
        ILocalizationService localizationService,
        IBasketRepository basketRepository, UserManager<ApplicationUser> userManager, IConfiguration configuration) : IServiceManager
    {
        private readonly Lazy<IProductService> _LazyProductService = new Lazy<IProductService>(() => new ProductService(unitOfWork, mapper, localizationService, entityAuditHelper));
        private readonly Lazy<IProductBrandService> _LazyProductBrandService = new Lazy<IProductBrandService>(() => new ProductBrandService(unitOfWork, mapper, localizationService, entityAuditHelper));
        private readonly Lazy<IProductTypeService> _LazyProductTypeService = new Lazy<IProductTypeService>(() => new ProductTypeService(unitOfWork, mapper, localizationService, entityAuditHelper));
        private readonly Lazy<IBasketService> _LazyBasketService = new Lazy<IBasketService>(() => new BasketService(basketRepository, mapper));
        private readonly Lazy<IAuthenticationService> _LazyAuthenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, configuration, mapper));
        private readonly Lazy<IOrderService> _LazyOrderService = new Lazy<IOrderService>(() => new OrderService(mapper, basketRepository, unitOfWork, entityAuditHelper));
        private readonly Lazy<IDeliveryMethodService> _LazyDeliveryMethodService = new Lazy<IDeliveryMethodService>(() => new DeliveryMethodService(unitOfWork, mapper, entityAuditHelper));
        public IProductService ProductService => _LazyProductService.Value;

        public IProductBrandService ProductBrandService => _LazyProductBrandService.Value;

        public IProductTypeService ProductTypeService => _LazyProductTypeService.Value;
        public IBasketService BasketService => _LazyBasketService.Value;
        public IAuthenticationService AuthenticationService => _LazyAuthenticationService.Value;
        public IOrderService OrderService => _LazyOrderService.Value;
        public IDeliveryMethodService DeliveryMethodService => _LazyDeliveryMethodService.Value;
    }
}
