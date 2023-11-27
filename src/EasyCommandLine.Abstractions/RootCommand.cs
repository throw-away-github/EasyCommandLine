using EasyCommandLine.Interfaces;
using static EasyCommandLine.DynTypes;

namespace EasyCommandLine;

/// <summary>
/// Represents the main action that the application performs.
/// </summary>
/// <typeparam name="TOptions">The options which are passed to the handler.</typeparam>
/// <typeparam name="TOptionsHandler">The handler which will be executed.</typeparam>
public abstract class RootCommand<
    [DynAccess(PublicTypes)] TOptions,
    [DynAccess(PublicConstructors)] TOptionsHandler> : System.CommandLine.CliRootCommand
    where TOptions : class, ICommandOptions
    where TOptionsHandler : class, ICommandOptionsHandler<TOptions>
{
    /// <inheritdoc />
    protected RootCommand(string description) : base(description)
    {
        Action = CommandHandlerProxy.HandleAsync<TOptions, TOptionsHandler>();
    }
}