using DynAccess = System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute;
using static EasyCommandLine.Core.DynamicAttributes;

namespace EasyCommandLine.Core.Interfaces;

[DynAccess(PublicTypes)]
public interface ICommandOptions
{
}