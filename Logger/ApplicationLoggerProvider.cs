using System.Collections.Concurrent;
using Microsoft.Extensions.Options;

namespace YTMP3DownloadAPI.Logger;
public class ApplicationLoggerProvider : ILoggerProvider
{
    private readonly IDisposable onChangeToken;
    private ApplicationLoggerConfiguration currentConfig;
    private readonly ConcurrentDictionary<string, ILogger> loggers = new(StringComparer.OrdinalIgnoreCase);

    private ApplicationLoggerConfiguration GetCurrentConfig() => currentConfig;

    public ApplicationLoggerProvider(IOptionsMonitor<ApplicationLoggerConfiguration> config)
    {
        currentConfig = config.CurrentValue;
        onChangeToken = config.OnChange(updateConfig => currentConfig = updateConfig);
    }

    public ILogger CreateLogger(string categoryName)
    {
        return loggers.GetOrAdd(categoryName, name => new ApplicationLogger(name, GetCurrentConfig));
    }

    public void Dispose()
    {
        loggers.Clear();
        onChangeToken.Dispose();
    }
}