using EasyCommandLine.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DynAccess = System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute;
using static EasyCommandLine.Core.DynamicAttributes;

namespace EasyCommandLine.Core;

public abstract class Command<
    [DynAccess(PublicTypes)] TOptions,
    [DynAccess(PublicConstructors)] TOptionsHandler> : System.CommandLine.Command
    where TOptions : class, ICommandOptions
    where TOptionsHandler : class, ICommandOptionsHandler<TOptions>
{
    protected Command(string name, string description) : base(name, description)
    {
        Handler = AotHandler.Create<TOptions, IHost, CancellationToken>(RunAsync);
    }

    private static async Task<int> RunAsync(TOptions options, IHost host, CancellationToken token)
    {
        var handler = ActivatorUtilities.CreateInstance<TOptionsHandler>(host.Services);
        return await handler.HandleAsync(options, token);
    }
}