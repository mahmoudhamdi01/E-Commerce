using Azure;
using CoreLayer.Exceptions;
using SharedLayer.ErrorModels;

namespace Talabat.Web.CustomMiddlewares
{
    public class CustomExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandler> _logger;

        public CustomExceptionHandler(RequestDelegate Next, ILogger<CustomExceptionHandler> logger)
        {
            _next = Next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext _httpContext)
        {
            try
            {
                await _next.Invoke(_httpContext);
                if(_httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    var Response = new ErrorToReturn()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        ErrorMessage = $"End Point {_httpContext.Request.Path} is Not Found"
                    };

                    await _httpContext.Response.WriteAsJsonAsync(Response);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Something Went Wrong");

                _httpContext.Response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    UnAuthorizedException => StatusCodes.Status401Unauthorized,
                    BadRequestException badRequestException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };

                var Response = new ErrorToReturn()
                {
                    StatusCode = _httpContext.Response.StatusCode,
                    ErrorMessage = ex.Message
                };

                await _httpContext.Response.WriteAsJsonAsync(Response);
            }
        }
    }
}
