global using DynAccess = System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute;
using Types = System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace EasyCommandLine;

internal static class DynTypes
{
    internal const Types PublicTypes = Types.PublicConstructors | Types.PublicProperties;
    internal const Types PublicConstructors = Types.PublicConstructors;

    internal const Types Public = Types.PublicFields |
                                         Types.PublicProperties |
                                         Types.PublicMethods |
                                         Types.Interfaces |
                                         Types.PublicEvents;
}