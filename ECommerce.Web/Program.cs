
using ECommerce.Application.Helpers;
using ECommerce.Application.MappingProfiles;
using ECommerce.Application.Repositories;
using ECommerce.Infrastructure.Contexts;
using ECommerce.Infrastructure.Entities.IdentityModule;
using ECommerce.Interface.Interfaces;
using ECommerce.Web.CustomMiddlewares;
using ECommerce.Web.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ── Extension Methods ────────────────────────────────────────────────
            builder.Services.AddPresentationAndSwagger();
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddIdentityAndJwt(builder.Configuration);

            // ── Persistence ──────────────────────────────────────────────────────
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // ────────────────────────────────────────────────────────────────────
            var app = builder.Build();

            // ── Data Seeding ─────────────────────────────────────────────────────
            //using (var scope = app.Services.CreateScope())
            //{
            //    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeding>();
            //    await seeder.DataSeedAsync();
            //    await seeder.IdentityDataSeedAsync();
            //}

            // ── Middleware Pipeline ──────────────────────────────────────────────
            app.UseMiddleware<LanguageMiddleware>();
            app.UseMiddleware<CustomExceptionHandler>();
            app.UseMiddleware<JwtCookieToHeaderMiddleware>();
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwaggerWithUI();   // Swagger + UI + root redirect "/"
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
