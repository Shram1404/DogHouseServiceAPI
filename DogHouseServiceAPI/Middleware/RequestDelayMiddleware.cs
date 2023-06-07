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

    public async Task Invoke(HttpContext context)
    {
        var currentTime = DateTime.Now;

        // Перевірити, чи минув достатній час з останнього запиту
        if (currentTime - _lastRequestTime >= TimeSpan.FromSeconds(1))
        {
            _requestCount = 1;
            _lastRequestTime = currentTime;
        }
        else
        {
            _requestCount++;
            if (_requestCount > _limit)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                var errorObject = new { Error = "Too many requests. Please try again later" };
                await context.Response.WriteAsJsonAsync(errorObject);
                return;
            }
        }
        await _next(context);
    }
}
