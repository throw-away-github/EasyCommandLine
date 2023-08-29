using DynAccess = System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute;
using static EasyCommandLine.Core.DynamicAttributes;

namespace EasyCommandLine.Core.Interfaces;

/// <summary>
/// Represents the options which a command accepts.
/// </summary>
[DynAccess(PublicTypes)]
public interface ICommandOptions
{
}