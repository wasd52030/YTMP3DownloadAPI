using YoutubeExplode.Videos.Streams;
using System.Text.RegularExpressions;
using FFMpegCore;
using FFMpegCore.Pipes;


public static class Extensions
{

    public static async Task<Stream> GetMp3Stream(this IStreamInfo streamInfo)
    {
        var res = new MemoryStream();

        await FFMpegArguments.FromUrlInput(new Uri(streamInfo.Url))
                             .OutputToPipe(new StreamPipeSink(res), 
                                            options => options.ForceFormat("mp3"))
                             .ProcessAsynchronously();
        res.Position = 0;

        return res;
    }

    public static Stream AnnotateMp3Tag(this Stream stream, string title, string? comment)
    {
        TagLib.Id3v2.Tag.DefaultVersion = 4;
        TagLib.Id3v2.Tag.ForceDefaultVersion = true;

        string pattern = @"\[(.*?)\]";
        var matches = Regex.Matches(title, pattern);

        try
        {
            using var mp3 = TagLib.File.Create(new StreamFileAbstraction(title, stream));

            if (matches.Any())
            {
                var contributors = matches.Select(match => match.Groups[1].Value);

                mp3.Tag.Performers = contributors.ToArray();
            }

            if (comment != null)
            {
                mp3.Tag.Comment = comment;
            }

            mp3.Save();
        }
        catch (System.Exception)
        {
            throw;
        }

        return stream;
    }
}