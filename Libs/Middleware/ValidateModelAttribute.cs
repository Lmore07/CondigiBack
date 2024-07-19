using CondigiBack.Libs.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace CondigiBack.Libs.Middleware
{
    public class ValidateModelAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = new List<string>();

                foreach (var entry in context.ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        string customMessage = $"Campo {entry.Key}: {error.ErrorMessage}";
                        errors.Add(customMessage);
                    }
                }

                var errorResponse = new BadRequestResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Error = "Se han encontrado errores de validación.",
                    Errors = errors
                };

                context.Result = new BadRequestObjectResult(errorResponse);
            }
        }
    }
}
