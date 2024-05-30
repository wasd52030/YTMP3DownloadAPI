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

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
    options.DocumentTitle = "youtube mp3 download api document";
});


async Task<IResult> downloadWithvideoName(
    [SwaggerParameter("youtube影片網址")]
    string videoUrl
)
{
    try
    {
        // parse url in route param videourl
        videoUrl = HttpUtility.UrlDecode(videoUrl);

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
    [SwaggerParameter("youtube影片網址")]
    string videoUrl,
    [SwaggerParameter("自訂檔名")]
    string custName,
    [SwaggerParameter("comment")]
    string? comment
)
{

    try
    {
        // parse url in route param videourl
        videoUrl = HttpUtility.UrlDecode(videoUrl);

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


app.MapGet("/download/{videoUrl}", downloadWithvideoName);
app.MapGet("/download/{videoUrl}/{custName}", downloadWithCustomName);
app.MapGet("/a", () => new { status = 200, message = "動了，它動了" });

app.Run();