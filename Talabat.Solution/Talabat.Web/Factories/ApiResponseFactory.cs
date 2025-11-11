using Microsoft.AspNetCore.Mvc;
using SharedLayer.ErrorModels;

namespace Talabat.Web.Factories
{
    public class ApiResponseFactory
    {
        public static IActionResult GenerateApiValidationErrorsResponse(ActionContext context)
        {
            var Errors = context.ModelState.Where(M => M.Value.Errors.Any())
                   .Select(M => new ValidationError()
                   {
                       Field = M.Key,
                       Errors = M.Value.Errors.Select(E => E.ErrorMessage)
                   });
            var Response = new ValidationErrorToReturn()
            {
                ValidationErrors = Errors,
            };

            return new BadRequestObjectResult(Response);
        }
    }
}
