using System.CommandLine.Hosting;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace EasyCommandLine.Extensions;

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
}