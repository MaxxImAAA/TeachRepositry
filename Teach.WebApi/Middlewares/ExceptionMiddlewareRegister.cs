namespace Teach.WebApi.Middlewares
{
    public static class ExceptionMiddlewareRegister
    {
        public static IApplicationBuilder AddExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            return app;
        }

    }
}
