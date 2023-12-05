namespace Project.Web.Middlewares
{
    public class SessionCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context != null && context.Session != null)
            {
                if (!context.Session.TryGetValue("UserSessionData", out _))
                {
                    // Redirect to login page if no session data found
                    context.Response.Redirect("/Account/Logout");
                    return;
                }
            }

            await _next(context);
        }
    }
}
