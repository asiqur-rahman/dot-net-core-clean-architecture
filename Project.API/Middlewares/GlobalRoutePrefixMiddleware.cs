using System.Reflection.Metadata;
using System.Text;

namespace Project.API.Middlewares
{
    public class GlobalRoutePrefixMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _routePrefix;

        // The constructor accepts RequestDelegate parameter and a string parameter
        // representing the desired global prefix.The InvokeAsync method serves as
        // the entry point for the middleware’s execution and accepts the HttpContext.
        // This method is responsible for modifying the PathBase property of the incoming
        // request in order to add a global prefix.
        public GlobalRoutePrefixMiddleware(RequestDelegate next, string routePrefix)
        {
            _next = next;
            _routePrefix = routePrefix;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.PathBase = new PathString(_routePrefix);
            await _next(context);
        }
    }
}
