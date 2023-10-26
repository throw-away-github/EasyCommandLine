using System.CommandLine.Hosting;
using System.Text.Json.Serialization.Metadata;
using EasyCommandLine.Interfaces;
using EasyCommandLine.Config;
using EasyCommandLine.Extensions.Spectre;
using EasyCommandLine.Logging;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Vertical.SpectreLogger;

namespace EasyCommandLine;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>.
/// </summary>
[UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Suppresses startup and shutdown messages from the host.
    /// </summary>
    public static IServiceCollection SuppressStatusMessages(this IServiceCollection services)
    {
        return services.Configure<InvocationLifetimeOptions>(options => 
            options.SuppressStatusMessages = true);
    }
    
    public static IServiceCollection AddConfig<T>(this IServiceCollection services, string configPath,
        JsonTypeInfo<T> typeInfo) where T : new()
    {
        return services.AddSingleton<IConfigContext<T>>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<ConfigContext<T>>>();
            var file = new FileInfo(configPath);
            return new ConfigContext<T>(logger, typeInfo, file);
        });
    }
    
    public static ILoggingBuilder AddAnsiLogger(
        this ILoggingBuilder logging,
        Action<AnsiLoggerOptionsBuilder>? configureOptions)
    {
        var services = logging.Services;
        var optionsBuilder = new AnsiLoggerOptionsBuilder(services);

        logging.ConfigurePrettyConsole();
        logging.ClearProviders();
        services.AddSingleton<AnsiLoggerProvider>();
        services.AddSingleton<ILoggerProvider>(sp => sp.GetRequiredService<AnsiLoggerProvider>());
        services.AddSingleton<IAnsiLoggerProvider>(sp => sp.GetRequiredService<AnsiLoggerProvider>());
        
        services.TryAdd(ServiceDescriptor.Singleton(typeof(IAnsiLogger<>), typeof(AnsiLogger<>)));
        services.AddSingleton<SpectreLoggerProvider>();
        
        configureOptions?.Invoke(optionsBuilder);
        return logging;
    }
}