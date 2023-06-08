using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace DogHouseServiceAPI;

public class RequestLimitMiddlewareTests
{
    [Fact]
    public async Task Invoke_Should_ReturnTooManyRequests_When_RequestLimitExceeded()
    {
        // Arrange
        var limit = 5; // Max requests per second

        // Create an instance of the RequestLimitMiddleware with a callback function for the next step of request processing
        var middleware = new RequestLimitMiddleware(next: (innerHttpContext) =>
        {
            return Task.CompletedTask;
        }, limit: limit);

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Send requests up to the limit to simulate exceeding the request limit
        for (var i = 0; i < limit; i++)
            await middleware.Invoke(context);

        var expectedStatusCode = (int)HttpStatusCode.TooManyRequests;

        // Act
        await middleware.Invoke(context);

        // Assert
        Assert.Equal(expectedStatusCode, context.Response.StatusCode);
    }

}
