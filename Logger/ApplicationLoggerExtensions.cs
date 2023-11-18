using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;

namespace YTMP3DownloadAPI.Logger;

/// <summary>
/// static class for ApplicationLogger Extension method
/// </summary>
public static class ApplicationLoggerExtensions
{
    /// <summary>
    /// AddApplicationLogger Extension method
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static ILoggingBuilder AddApplicationLogger(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ApplicationLoggerProvider>());

        LoggerProviderOptions.RegisterProviderOptions<ApplicationLoggerConfiguration, ApplicationLoggerProvider>(builder.Services);

        return builder;
    }

    /// <summary>
    /// AddApplicationLogger Extension method with logger configure
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static ILoggingBuilder AddApplicationLogger(this ILoggingBuilder builder, Action<ApplicationLoggerConfiguration> configure)
    {
        builder.AddApplicationLogger();
        builder.Services.Configure(configure);
        return builder;
    }
}