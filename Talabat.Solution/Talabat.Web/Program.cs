
using CoreLayer.Entities.IdentityModule;
using CoreLayer.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PersistanceLayer;
using PersistanceLayer.Data.Context;
using PersistanceLayer.Identity;
using PersistanceLayer.Repositories;
using ServiceAbstractionLayer;
using ServiceLayer;
using ServiceLayer.MappingProfile;
using SharedLayer.ErrorModels;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using Talabat.Web.CustomMiddlewares;
using Talabat.Web.Factories;
using Microsoft.OpenApi.Models;

namespace Talabat.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(Options =>
            {
                Options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    Description = "'Enter Bearer' Followed By Space And Your Token"
                });
                Options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                              Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                    },
                            new string[] {}
                        }
                });
            });
            builder.Services.AddDbContext<ApplicationDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<StoreIdentityDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDefaultConnection"));
            });
            builder.Services.AddAutoMapper(typeof(ProductProfile));
            builder.Services.AddAutoMapper(typeof(IdentityProfile));
            builder.Services.AddAutoMapper(typeof(OrderProfile));
            builder.Services.AddScoped<IDataSeeding, DataSeeding>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICacheService, CacheService>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddSingleton<IConnectionMultiplexer>((_) =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnectionString"));
            });
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.Configure<ApiBehaviorOptions>((Options) => {
                Options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationErrorsResponse;
            });
            builder.Services.AddIdentityCore<ApplicationUser>()
                            .AddRoles<IdentityRole>()
                            .AddEntityFrameworkStores<StoreIdentityDbContext>();

            builder.Services.AddAuthentication(Options =>
            {
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWTOptions:ISSuer"],

                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWTOptions:Audience"],

                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:SecretKey"]))
                };
            });
            var app = builder.Build();

            #region Apply Data Seeding
            using var Scope = app.Services.CreateScope();
            var Services = Scope.ServiceProvider;
            var objects = Services.GetRequiredService<IDataSeeding>();
            await objects.DataSeedAsync();
            await objects.IdentityDataSeedAsync();
            #endregion

            // Configure the HTTP request pipeline.
            app.UseMiddleware<CustomExceptionHandler>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(Options =>
                {
                    Options.ConfigObject = new ConfigObject()
                    {
                        DisplayRequestDuration = true
                    };
                    Options.DocumentTitle = "My E-Commerce API";
                    Options.DocExpansion(DocExpansion.None);
                    Options.EnableFilter();
                    Options.EnablePersistAuthorization();
                });
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
