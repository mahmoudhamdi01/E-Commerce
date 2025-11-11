using CoreLayer.Entities;
using CoreLayer.Entities.IdentityModule;
using CoreLayer.Entities.OrderModule;
using CoreLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using PersistanceLayer.Data.Context;
using PersistanceLayer.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PersistanceLayer
{
    public class DataSeeding(ApplicationDbContext _dbContext
        ,UserManager<ApplicationUser> _userManager,
        RoleManager<IdentityRole> _roleManager,
        StoreIdentityDbContext _identityDbContext) : IDataSeeding
    {
        public async Task DataSeedAsync()
        {
            if(!_dbContext.productBrands.Any())
            {
                var ProductBrands = File.ReadAllText("../PersistanceLayer/Data/DataSeeding/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(ProductBrands);
                if (Brands?.Count() > 0)
                {
                    foreach (var Brand in Brands)
                    {
                       await  _dbContext.AddAsync(Brand);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }

            if (!_dbContext.productTypes.Any())
            {
                var ProductTypes = File.ReadAllText("../PersistanceLayer/Data/DataSeeding/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(ProductTypes);
                if (Types?.Count() > 0)
                {
                    foreach (var Type in Types)
                    {
                        await _dbContext.AddAsync(Type);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }

            if (!_dbContext.products.Any())
            {
                var ProductsData = File.ReadAllText("../PersistanceLayer/Data/DataSeeding/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                if (Products?.Count() > 0)
                {
                    foreach (var product in Products)
                    {
                        await _dbContext.AddAsync(product);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }

            if (!_dbContext.Set<DeliveryMethod>().Any())
            {
                var DeliveryMethods = File.ReadAllText("../PersistanceLayer/Data/DataSeeding/delivery.json");
                var DeliveryMethodsData = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethods);
                if (DeliveryMethodsData?.Count() > 0)
                {
                    foreach (var deliveryMethod in DeliveryMethodsData)
                    {
                        await _dbContext.Set<DeliveryMethod>().AddAsync(deliveryMethod);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task IdentityDataSeedAsync()
        {
            if(!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }

            if(!_userManager.Users.Any())
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
            await _identityDbContext.SaveChangesAsync();
        }
    }
}
// D:\New folder (3)\Talabat\Talabat.Solution\PersistanceLayer\Data\DataSeeding\brands.json