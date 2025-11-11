using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServiceAbstractionLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Attributes
{
    public class CacheAttribute(int DurationInSec = 90) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string CacheKey = CreateCacheKey(context.HttpContext.Request);

            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var CacheValue = await cacheService.GetAsync(CacheKey);

            if(CacheValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = CacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            var ExecutedContext = await next.Invoke();

            if(ExecutedContext.Result is OkObjectResult result)
            {
                await cacheService.SetAsync(CacheKey, result.Value, TimeSpan.FromSeconds(DurationInSec));
            }
        }

        private string CreateCacheKey(HttpRequest request)
        {
            StringBuilder Key = new StringBuilder();
            Key.Append(request.Path + '?');
            foreach (var item in request.Query.OrderBy(Q=>Q.Key))
            {
                Key.Append($"{item.Key}={item.Value}&");
            }
            return Key.ToString();
        }
    }
}
