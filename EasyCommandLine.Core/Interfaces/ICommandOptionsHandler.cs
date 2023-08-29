using DynAccess = System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute;
using static EasyCommandLine.Core.DynamicAttributes;

namespace EasyCommandLine.Core.Interfaces;

/// <summary>
/// Performs an action when the associated command is invoked on the command line.
/// </summary>
/// <typeparam name="TOptions">The options which are passed to the handler.</typeparam>
[DynAccess(PublicConstructors)]
public interface ICommandOptionsHandler<in TOptions>
{
    /// <summary>
    /// Called when the associated command is invoked on the command line.
    /// </summary>
    /// <param name="options">The options which were passed to the command.</param>
    /// <param name="cancellationToken">Used to signal that the command should be cancelled (ctrl+c).</param>
    /// <returns>The exit code of the command.</returns>
    Task<int> HandleAsync(TOptions options, CancellationToken cancellationToken);
}