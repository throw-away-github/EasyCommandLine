using EasyCommandLine.Core.Interfaces;
using DynAccess = System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute;
using static EasyCommandLine.Core.DynamicAttributes;

namespace EasyCommandLine.Core;

/// <summary>
/// Represents a specific action that the application performs.
/// </summary>
/// <typeparam name="TOptions">The options which are passed to the handler.</typeparam>
/// <typeparam name="TOptionsHandler">The handler which will be executed.</typeparam>
public abstract class Command<
    [DynAccess(PublicTypes)] TOptions,
    [DynAccess(PublicConstructors)] TOptionsHandler> : System.CommandLine.Command
    where TOptions : class, ICommandOptions
    where TOptionsHandler : class, ICommandOptionsHandler<TOptions>
{
    /// <inheritdoc />
    protected Command(string name, string description) : base(name, description)
    {
        Handler = CommandHandlerProxy.HandleAsync<TOptions, TOptionsHandler>();
    }
}