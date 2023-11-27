using EasyCommandLine.Interfaces;
using static EasyCommandLine.DynTypes;

namespace EasyCommandLine;

/// <summary>
/// Represents a specific action that the application performs.
/// </summary>
/// <typeparam name="TOptions">The options which are passed to the handler.</typeparam>
/// <typeparam name="TOptionsHandler">The handler which will be executed.</typeparam>
public abstract class Command<
    [DynAccess(PublicTypes)] TOptions,
    [DynAccess(PublicConstructors)] TOptionsHandler> : System.CommandLine.CliCommand
    where TOptions : class, ICommandOptions
    where TOptionsHandler : class, ICommandOptionsHandler<TOptions>
{
    /// <inheritdoc />
    protected Command(string name, string description) : base(name, description)
    {
       Action = CommandHandlerProxy.HandleAsync<TOptions, TOptionsHandler>();
    }
}