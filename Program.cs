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
builder.Services.AddHttpLogging(logging=>{
    logging.LoggingFields=HttpLoggingFields.Request;
});

var app = builder.Build();

app.UseRequestLog();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
    options.DocumentTitle="youtube mp3 download api document";
});


async Task<IResult> downloadWithvideoName(
    [SwaggerParameter("youtube影片網址")]
    string videoUrl
)
{
    try
    {
        var serviceRes = await downloadService.download(videoUrl, null);

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


async Task<IResult> downloadWithCustomName(
    [SwaggerParameter("youtube影片網址")]
    string videoUrl,
    [SwaggerParameter("自訂檔名")]
    string custName
)
{

    try
    {
        var serviceRes = await downloadService.download(videoUrl, custName);

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


app.MapGet("/download/{videoUrl}", downloadWithvideoName);
app.MapGet("/download/{videoUrl}/{custName}", downloadWithCustomName);

app.Run();