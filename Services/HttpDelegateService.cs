namespace KevDevTools.Services
{
    public class HttpDelegateService
    {
        private readonly RequestDelegate _next;

        public HttpDelegateService(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //Ensure the session is loaded.
            await context.Session.LoadAsync();
            // Do something with context near the beginning of request processing.
            await _next(context);
            //Commit the session to the store.
            await context.Session.CommitAsync();
        }
    }
}
