using ECommerce.Interface.IServices.Authentication;
using ECommerce.Interface.IServices.Basket;
using ECommerce.Interface.IServices.DeliveryMethod;
using ECommerce.Interface.IServices.Order;
using ECommerce.Interface.IServices.Product;
using ECommerce.Interface.IServices.ProductBrand;
using ECommerce.Interface.IServices.ProductType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Interface.Interfaces
{
    public interface IServiceManager
    {
        public IProductService ProductService { get; }
        public IProductBrandService ProductBrandService { get; }
        public IProductTypeService ProductTypeService { get; }
        public IBasketService BasketService { get; }
        public IAuthenticationService AuthenticationService { get; }
        public IOrderService OrderService { get; }
        public IDeliveryMethodService DeliveryMethodService { get; }
    }
}
