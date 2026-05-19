using ECommerce.Infrastructure.Contexts;
using ECommerce.Infrastructure.Entities.IdentityModule;
using ECommerce.Infrastructure.Entities.OrderModule;
using ECommerce.Infrastructure.Entities.Products;
using ECommerce.Interface.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Application.Repositories
{
    public class DataSeeding(ApplicationDbContext _dbContext,
        UserManager<ApplicationUser> _userManager,
        RoleManager<IdentityRole> _roleManager) : IDataSeeding
    {
        public async Task DataSeedAsync()
        {
            if(_dbContext.Database.GetPendingMigrations().Any())
                _dbContext.Database.Migrate();

            if(!_dbContext.ProductBrands.Any())
            {
                var ProductBrands = File.ReadAllText(@"..\ECommerce.Infrastructure\DataSeed\brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(ProductBrands);
                if(Brands?.Count() > 0)
                    await _dbContext.ProductBrands.AddRangeAsync(Brands);
            }

            if (!_dbContext.ProductTypes.Any())
            {
                var ProductTypes = File.ReadAllText(@"..\ECommerce.Infrastructure\DataSeed\types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(ProductTypes);
                if (Types?.Count() > 0)
                    await _dbContext.ProductTypes.AddRangeAsync(Types);
            }

            if (!_dbContext.ProductBrands.Any())
            {
                var ProductsData = File.ReadAllText(@"..\ECommerce.Infrastructure\DataSeed\products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                if (Products?.Count() > 0)
                    await _dbContext.Products.AddRangeAsync(Products);
            }

            if (!_dbContext.DeliveryMethods.Any())
            {
                var DeliveryMethods = File.ReadAllText(@"..\ECommerce.Infrastructure\DataSeed\delivery.json");
                var DeliveryMethodsData = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethods);
                if (DeliveryMethodsData?.Count() > 0)
                {
                    DeliveryMethodsData.ForEach(d => d.Id = default); // ← امسح الـ Id
                    await _dbContext.DeliveryMethods.AddRangeAsync(DeliveryMethodsData);
                }
            }

            await _dbContext.SaveChangesAsync();    
        }

        public async Task IdentityDataSeedAsync()
        {
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }

            if (!_userManager.Users.Any())
            {
                var User01 = new ApplicationUser()
                {
                    Email = "mahmoudhamdi01@gmail.com",
                    DisplayName = "Mahmoud Hamdi",
                    UserName = "MahmoudHamdi",
                    PhoneNumber = "01234567890",
                };

                var User02 = new ApplicationUser()
                {
                    Email = "ahmedamr02@gmail.com",
                    DisplayName = "Ahmed Amr",
                    UserName = "AhmedAmr",
                    PhoneNumber = "01145100263",
                };

                await _userManager.CreateAsync(User01, "P@ssw0rd");
                await _userManager.CreateAsync(User02, "Pa$$w0rd");

                await _userManager.AddToRoleAsync(User01, "SuperAdmin");
                await _userManager.AddToRoleAsync(User02, "Admin");
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
