using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;

namespace YTMP3DownloadAPI.Logger;

public static class ApplicationLoggerExtensions
{
    public static ILoggingBuilder AddApplicationLogger(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ApplicationLoggerProvider>());

        LoggerProviderOptions.RegisterProviderOptions<ApplicationLoggerConfiguration, ApplicationLoggerProvider>(builder.Services);

        return builder;
    }

    public static ILoggingBuilder AddApplicationLogger(this ILoggingBuilder builder, Action<ApplicationLoggerConfiguration> configure)
    {
        builder.AddApplicationLogger();
        builder.Services.Configure(configure);
        return builder;
    }
}