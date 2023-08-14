using DynAccess = System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute;
using static EasyCommandLine.Core.DynamicAttributes;

namespace EasyCommandLine.Core.Interfaces;

[DynAccess(PublicConstructors)]
public interface ICommandOptionsHandler<in TOptions>
{
    Task<int> HandleAsync(TOptions options, CancellationToken cancellationToken);
}