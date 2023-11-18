namespace YTMP3DownloadAPI.Logger;

/// <summary>
/// 紀錄Request資訊
/// 包含method、Path、Status、userAgent、contenType、ip
/// </summary>
public class RequestLogMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<RequestLogMiddleware> logger;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="next"></param>
    /// <param name="logger"></param>
    public RequestLogMiddleware(RequestDelegate next, ILogger<RequestLogMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    /// <summary>
    /// middleware logic
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task Invoke(HttpContext context)
    {
        var req = context.Request;
        var res = context.Response;

        logger.LogInformation("{} route:{} statusCode:{}", req.Method, req.Path, res.StatusCode);
        logger.LogInformation("userAgent: {} contenType: {} ip: {}", req.Headers["User-Agent"], req.ContentType, context.Connection.RemoteIpAddress);

        await next(context);
    }
}

/// <summary>
/// static class for UseRequestLog Extension method
/// </summary>
public static class RequestLogMiddlewareExtensions
{
    /// <summary>
    /// UseRequestLog Extension method
    /// </summary>
    /// <param name="app"></param>
    public static void UseRequestLog(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestLogMiddleware>();
    }
}