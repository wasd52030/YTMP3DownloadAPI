using System.Collections.Concurrent;
using Microsoft.Extensions.Options;

namespace YTMP3DownloadAPI.Logger;

/// <summary>
/// ApplicationLoggerProvider class
/// </summary>
public class ApplicationLoggerProvider : ILoggerProvider
{
    private readonly IDisposable onChangeToken;
    private ApplicationLoggerConfiguration currentConfig;
    private readonly ConcurrentDictionary<string, ILogger> loggers = new(StringComparer.OrdinalIgnoreCase);

    private ApplicationLoggerConfiguration GetCurrentConfig() => currentConfig;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="config"></param>
    public ApplicationLoggerProvider(IOptionsMonitor<ApplicationLoggerConfiguration> config)
    {
        currentConfig = config.CurrentValue;
        onChangeToken = config.OnChange(updateConfig => currentConfig = updateConfig);
    }

    /// <summary>
    /// get logger instance
    /// </summary>
    /// <param name="categoryName"></param>
    /// <returns></returns>
    public ILogger CreateLogger(string categoryName)
    {
        return loggers.GetOrAdd(categoryName, name => new ApplicationLogger(name, GetCurrentConfig));
    }

    /// <summary>
    /// release resource
    /// </summary>
    public void Dispose()
    {
        loggers.Clear();
        onChangeToken.Dispose();
    }
}