using System.CommandLine.Hosting;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace EasyCommandLine.Core.Extensions;

[UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection SuppressStatusMessages(this IServiceCollection services)
    {
        return services.Configure<InvocationLifetimeOptions>(options => 
            options.SuppressStatusMessages = true);
    }
}