using System.Web;
using Microsoft.AspNetCore.HttpLogging;
using Swashbuckle.AspNetCore.Annotations;
using YTMP3DownloadAPI.Logger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddApplicationLogger();
});

// swagger service
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.EnableAnnotations());

// Http request log service
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.Request;
});

var app = builder.Build();

app.UseRequestLog();

// http://localhost:5195/swagger/index.html
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    // options.RoutePrefix = string.Empty;
    options.DocumentTitle = "youtube mp3 download api document";
});


async Task<IResult> downloadWithvideoName(
    [SwaggerParameter("youtube影片ID")]
    string videoID
)
{
    try
    {
        // parse url in route param videourl
        var videoUrl = HttpUtility.UrlDecode($"https://www.youtube.com/watch?v={videoID}");

        var serviceRes = await downloadService.download(videoUrl, null, null);

        return Results.File(
            serviceRes.fileStream,
            contentType: "audio/mp3",
            fileDownloadName: serviceRes.fileName,
            enableRangeProcessing: true
        );
    }
    catch (System.Exception)
    {
        throw;
    }
}


async Task<IResult?> downloadWithCustomName(
    [SwaggerParameter("youtube影片ID")]
    string videoID,
    [SwaggerParameter("自訂檔名")]
    string custName,
    [SwaggerParameter("comment")]
    string? comment
)
{

    try
    {
        // parse url in route param videourl
        var videoUrl = HttpUtility.UrlDecode($"https://www.youtube.com/watch?v={videoID}");

        var serviceRes = await downloadService.download(videoUrl, custName, comment);
        var fileDownloadName = serviceRes.fileName.Split("]").Last().Trim();

        return Results.File(
            serviceRes.fileStream,
            contentType: "audio/mp3",
            fileDownloadName: fileDownloadName,
            enableRangeProcessing: true
        );
    }
    catch (System.Exception e)
    {
        throw;
    }
}

// TODO: 做一個簡易的前端對外
app.MapGet("/download/{videoID}", downloadWithvideoName);
app.MapGet("/download/{videoID}/{custName}", downloadWithCustomName);
app.MapGet("/a", () => new { status = 200, message = "動了，它動了" });

app.Run();