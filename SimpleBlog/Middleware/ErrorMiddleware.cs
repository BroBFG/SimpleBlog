namespace SimpleBlog.Middleware
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate next;
        private ILogger<ErrorMiddleware> logger;
        public ErrorMiddleware(RequestDelegate next, ILogger<ErrorMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //logger.LogInformation("ErrorMiddleware is connected");
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, "Catched by ErrorMiddleware Method:" + context.Request.Method);
            }
        }
    }
}
