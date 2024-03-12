using Microsoft.AspNetCore.Mvc;

namespace DndHpCalculator.Tests.Integration.API;

public class Helpers
{
    
}

public static class HttpAssertions
{
    public static void AssertOkResult<T>(IActionResult result, Action<T?>? resultAssertion) where T:class
    {
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        resultAssertion?.Invoke(okResult.Value as T);
    }

    public static void AssertNotFound(IActionResult responseResult)
    {
        var notFoundResult = Assert.IsType<NotFoundResult>(responseResult);
        Assert.Equal(404,notFoundResult.StatusCode);
    }
}