using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

class downloadService
{
    public record DownloadServiceResult(string fileName, Stream fileStream);

    static string removeSpecialChar(string input)
    {
        foreach (var item in new string[] { @"*", @"/", "\n", "\"", "|", ":", "?" })
        {
            input = input.Replace(item, "");
        }

        return input;
    }

    public static async Task<DownloadServiceResult> download(string url, string? custName, string? comment)
    {
        var yt = new YoutubeClient();
        var vinfo = await yt.Videos.GetAsync(url);
        // https://blog.miniasp.com/post/2007/10/28/New-operator-found-in-CSharp-question-mark
        var vtitle = custName ?? removeSpecialChar(vinfo.Title);

        var videotManifest = await yt.Videos.Streams.GetManifestAsync(url);
        var videoInfo = videotManifest.GetAudioOnlyStreams()
                                      .GetWithHighestBitrate();

        // https://learn.microsoft.com/zh-tw/dotnet/api/system.web.httputility.urlencode
        // var filename = @$"{HttpUtility.UrlEncode(vtitle)}.mp3".Replace("+", "%20").Replace("%2b", "+");
        var filename = @$"{vtitle}.mp3";

        var res = (await videoInfo.GetMp3Stream()).AnnotateMp3Tag(filename, comment);

        return new DownloadServiceResult(filename, res);
    }
}
