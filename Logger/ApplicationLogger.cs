using System.Globalization;

namespace YTMP3DownloadAPI.Logger;

class ApplicationLogger : ILogger
{
    private static object Lock = new object();
    private readonly string name;
    private readonly Func<ApplicationLoggerConfiguration> getCurrentConfig;

    public ApplicationLogger(string name, Func<ApplicationLoggerConfiguration> getCurrentConfig)
    {
        (this.name, this.getCurrentConfig) = (name, getCurrentConfig);
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

    public bool IsEnabled(LogLevel logLevel) => getCurrentConfig().LogLevels.ContainsKey(logLevel);

    private string GetTimestamp()
    {
        DateTime now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local);
        string str = now.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
        return str;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        lock (Lock)
        {
            var config = getCurrentConfig();
            if (config.EventId == 0 || config.EventId == eventId.Id)
            {
                var color = Console.ForegroundColor;
                var currentTimeStamp = GetTimestamp();
                Console.ForegroundColor = config.LogLevels[logLevel];
                Console.Write($"[{logLevel}] ");
                // Console.Write($"{name }");
                Console.ForegroundColor = color;
                Console.Write($" - {currentTimeStamp}    ");
                Console.ForegroundColor = config.LogLevels[logLevel];

                // Log the formatted message
                var message = $"{formatter(state, exception)}";

                // If an exception is present, log the exception details and stack trace
                if (exception != null)
                {
                    message += Environment.NewLine + $"Exception: {exception.GetType().FullName}";
                    message += Environment.NewLine + $"Message: {exception.Message}";
                    message += Environment.NewLine + $"Stack Trace: {exception.StackTrace}";
                }

                Console.WriteLine(message);

                Console.ForegroundColor = color;
            }
        }
    }
}