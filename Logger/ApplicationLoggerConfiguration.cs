namespace YTMP3DownloadAPI.Logger;

public class ApplicationLoggerConfiguration
{
    public int EventId { get; set; }
    public Dictionary<LogLevel, ConsoleColor> LogLevels { get; set; } = new()
    {
        [LogLevel.Trace] = ConsoleColor.White,
        [LogLevel.Debug] = ConsoleColor.Blue,
        [LogLevel.Error] = ConsoleColor.Red,
        [LogLevel.Warning] = ConsoleColor.Yellow,
        [LogLevel.Information] = ConsoleColor.Green,
        [LogLevel.Critical] = ConsoleColor.Cyan
    };
}