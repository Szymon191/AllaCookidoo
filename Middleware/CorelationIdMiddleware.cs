namespace AllaCookidoo.Middleware
{
    public class CorelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CorelationIdMiddleware> _logger;

        public CorelationIdMiddleware(RequestDelegate next, ILogger<CorelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers.Add("X-Correlation-ID", correlationId);
            }

            context.Response.Headers.Add("X-Correlation-ID", correlationId);

            using (_logger.BeginScope("CorrelationID: {CorrelationID}", correlationId))
            {
                await _next(context);
            }
        }
    }
}
