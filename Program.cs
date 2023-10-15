using System.ComponentModel;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.EnableAnnotations());

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

// async  downloadService(){}

async Task<IResult> downloadWithCustomName(
    [SwaggerParameter("youtube video id")]
    string videoId,
    [DefaultValue(null)]
    [SwaggerParameter("custom file name",Required =false)]
    string? custName
)
{
    var url = $"https://www.youtube.com/watch?v={videoId}";

    try
    {
        var serviceRes = await downloadService.download(url, custName);

        return Results.File(
            serviceRes.fileStream,
            contentType: "audio/mp3",
            fileDownloadName: serviceRes.fileName,
            enableRangeProcessing: true
        );
    }
    catch (System.Exception e)
    {
        throw;
    }
}

async Task<IResult> downloadWithvideoName(
    [SwaggerParameter("youtube video id")]
    string videoId
)
{
    var url = $"https://www.youtube.com/watch?v={videoId}";

    try
    {
        var serviceRes = await downloadService.download(url, null);

        return Results.File(
            serviceRes.fileStream,
            contentType: "audio/mp3",
            fileDownloadName: serviceRes.fileName,
            enableRangeProcessing: true
        );
    }
    catch (System.Exception e)
    {
        throw;
    }
}

app.MapGet("/download/{videoId}", downloadWithvideoName);
app.MapGet("/download/{videoId}/{custName?}", downloadWithCustomName);

app.Run();