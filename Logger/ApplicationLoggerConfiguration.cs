namespace YTMP3DownloadAPI.Logger;

/// <summary>
/// logger config
/// </summary>
public class ApplicationLoggerConfiguration
{
    /// <summary>
    /// eventid
    /// </summary>
    /// <value></value>
    public int EventId { get; set; }

    /// <summary>
    /// log levels
    /// </summary>
    /// <returns></returns>
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