using Microsoft.AspNetCore.Http;

namespace Project.API.Middlewares
{
    public class GlobalRoutePrefixMiddleware : IMiddleware
    {
        private readonly string _routePrefix;

        public GlobalRoutePrefixMiddleware(string routePrefix)
        {
            _routePrefix = routePrefix;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Request.PathBase = new PathString(_routePrefix);
            await next(context);
        }
    }
}