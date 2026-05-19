using ECommerce.Application.MappingProfiles;
using ECommerce.Application.Repositories;
using ECommerce.Application.Services;
using ECommerce.Interface.Interfaces;
using ECommerce.Interface.IServices.Cache;
using StackExchange.Redis;

namespace ECommerce.Web.Factories
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // AutoMapper - scans the entire assembly of ProductProfile
            services.AddAutoMapper(typeof(ProductProfile).Assembly);

            // HttpContext
            services.AddHttpContextAccessor();

            // Data Seeding
            services.AddScoped<IDataSeeding, DataSeeding>();

            // Core Repositories & Services
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IServiceManager, ServiceManager>();

            // Basket
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ICacheRepository, CacheRepository>();
            services.AddScoped<ICacheService, CacheService>();

            // Localization & Audit
            services.AddScoped<ILocalizationService, LocalizationService>();
            services.AddScoped<IEntityAuditHelper, EntityAuditHelper>();

            // Redis Connection (Singleton)
            services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(
                    configuration.GetConnectionString("RedisConnectionString")!));

            return services;
        }
    }
}
