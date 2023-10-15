using System.ComponentModel;
using Microsoft.AspNetCore.HttpLogging;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

// swagger service
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.EnableAnnotations());

// Http request log service
builder.Services.AddHttpLogging(logging=>{
    logging.LoggingFields=HttpLoggingFields.Request;
});

var app = builder.Build();

app.UseHttpLogging();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
    options.DocumentTitle="youtube mp3 download api document";
});


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


async Task<IResult> downloadWithCustomName(
    [SwaggerParameter("youtube video id")]
    string videoId,
    [SwaggerParameter("custom file name")]
    string custName
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


app.MapGet("/download/{videoId}", downloadWithvideoName);
app.MapGet("/download/{videoId}/{custName}", downloadWithCustomName);

app.Run();