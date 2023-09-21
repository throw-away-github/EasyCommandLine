using DynAOT = System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace EasyCommandLine.Core;

internal static class DynamicAttributes
{
    internal const DynAOT PublicTypes = DynAOT.PublicConstructors | DynAOT.PublicProperties;
    internal const DynAOT PublicConstructors = DynAOT.PublicConstructors;

    internal const DynAOT Public = DynAOT.PublicFields |
                                         DynAOT.PublicProperties |
                                         DynAOT.PublicMethods |
                                         DynAOT.Interfaces |
                                         DynAOT.PublicEvents;
}