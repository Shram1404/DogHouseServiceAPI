public class RequestLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly int _limit;
    private DateTime _lastRequestTime = DateTime.MinValue;
    private int _requestCount = 0;

    public RequestLimitMiddleware(RequestDelegate next, int limit)
    {
        _next = next;
        _limit = limit;
    }
    /// <summary>
    /// Middleware to limit the number of requests per second.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    public async Task Invoke(HttpContext context)
    {
        var currentTime = DateTime.Now;

        if (currentTime - _lastRequestTime >= TimeSpan.FromSeconds(1)) // 1 second time frame
        {
            // Reset the request count if the time frame has elapsed
            _requestCount = 1;
            _lastRequestTime = currentTime;
        }
        else
        {
            // Increment the request count if still within the time frame
            _requestCount++;
            if (_requestCount > _limit)
            {
                // Return a 429 Too Many Requests response if the limit is exceeded
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                var errorObject = new { Error = "Too many requests. Please try again later" };
                await context.Response.WriteAsJsonAsync(errorObject);
                return;
            }
        }

        // Call the next middleware in the pipeline
        await _next(context);
    }
}
