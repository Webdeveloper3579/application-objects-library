using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AOL_Portal.Data
{
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyHeaderName = "X-API-UMFAS-KEY";
        private static string? _apiKey;

        public static void Configure(IConfiguration configuration)
        {
            _apiKey = configuration["ApiService:ApiKey"];
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_apiKey == null)
            {
                context.Result = new StatusCodeResult(500); // Misconfiguration
                return;
            }
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey) ||
                !string.Equals(extractedApiKey, _apiKey, StringComparison.Ordinal))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            await next();
        }
    }
}
