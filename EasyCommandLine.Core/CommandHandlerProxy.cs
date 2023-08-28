using System.CommandLine.Invocation;
using EasyCommandLine.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DynAccess = System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute;
using static EasyCommandLine.Core.DynamicAttributes;

namespace EasyCommandLine.Core;

public static class CommandHandlerProxy
{
    public static ICommandHandler HandleAsync<[DynAccess(PublicTypes)] T, [DynAccess(PublicConstructors)] T2>()
        where T : class, ICommandOptions
        where T2 : class, ICommandOptionsHandler<T>
    {
        return AotHandler.Create<T, IHost, CancellationToken>(RunAsync);
        
        async Task<int> RunAsync(T options, IHost host, CancellationToken token)
        {
            // The handler is created using the service provider from the host.
            // This allows the handler to use true dependency injection, while remaining within the constraints of System.CommandLine.
            var handler = ActivatorUtilities.CreateInstance<T2>(host.Services);
            return await handler.HandleAsync(options, token);
        }
    }
}