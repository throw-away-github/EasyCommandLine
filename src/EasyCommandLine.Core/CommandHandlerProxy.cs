using System.CommandLine.NamingConventionBinder;
using EasyCommandLine.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DynAccess = System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute;
using static EasyCommandLine.Core.DynamicAttributes;

namespace EasyCommandLine.Core;

public static class CommandHandlerProxy
{
    public static BindingHandler HandleAsync<[DynAccess(PublicTypes)] T, [DynAccess(PublicConstructors)] T2>()
        where T : class, ICommandOptions
        where T2 : class, ICommandOptionsHandler<T>
    {
        return AotHandler.Create<T, IHost, CancellationToken>(RunAsync);

        async Task<int> RunAsync(T options, IHost host, CancellationToken token)
        {
            // The handler is created using the service provider from the host.
            // This allows the handler to use true dependency injection, while remaining within the constraints of System.CommandLine.
            try
            {
                T2 handler;
                if (host.Services.GetService<IServiceCollection>() is { } serviceCollection)
                {
                    serviceCollection.AddSingleton(options);
                    serviceCollection.AddTransient<T2>();
                    handler = serviceCollection.BuildServiceProvider().GetRequiredService<T2>();
                }
                else
                {
                    handler = ActivatorUtilities.CreateInstance<T2>(host.Services);
                }
                
                return await handler.HandleAsync(options, token);
            }
            catch (Exception e)
            {
                var logger = host.Services.GetRequiredService<ILogger<T2>>();
                if (e is not OperationCanceledException)
                {
                    logger.LogError(e, "An unhandled exception occurred while executing the command");
                    return 1;
                }

                logger.LogWarning("The command was cancelled");
                return 0;
            }
        }
    }
}