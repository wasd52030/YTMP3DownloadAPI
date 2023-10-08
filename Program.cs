using System.Web;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

string removeSpecialChar(string input)
{
    foreach (var item in new string[] { @"*", @"/", "\n", "\"", "|", ":", "?" })
    {
        input = input.Replace(item, "");
    }

    return input;
}

app.MapGet("/", () => "Hello World!");


app.MapGet("/download/{videoId}/{custName?}", async (string videoId, string? custName) =>
{
    var url = $"https://www.youtube.com/watch?v={videoId}";

    try
    {
        var yt = new YoutubeClient();
        var vinfo = await yt.Videos.GetAsync(url);
        // https://blog.miniasp.com/post/2007/10/28/New-operator-found-in-CSharp-question-mark
        var vtitle = custName ?? removeSpecialChar(vinfo.Title);

        var videotManifest = await yt.Videos.Streams.GetManifestAsync(url);
        var videoInfo = videotManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

        var res = await yt.Videos.Streams.GetAsync(videoInfo);

        // https://learn.microsoft.com/zh-tw/dotnet/api/system.web.httputility.urlencode
        var filename = @$"{HttpUtility.UrlEncode(vtitle)}.mp3".Replace("+", "%20").Replace("%2b", "+");

        return Results.File(
            res,
            contentType: "audio/mp3",
            fileDownloadName: filename,
            enableRangeProcessing: true
        );
    }
    catch (System.Exception e)
    {
        throw;
    }
});

app.Run();
