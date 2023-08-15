using System.CommandLine;
using JetBrains.Annotations;

namespace EasyCommandLine.Core.Extensions;

[UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
public static class OptionExtensions
{
    public static T WithShortAlias<T>(this T option) where T : Option
    {
        return option.Name.Length > 1
            ? option.WithShortAlias(option.Name[0]) 
            : option;
    }
    
    public static T WithShortAlias<T>(this T option, char alias) where T : Option
    {
        option.AddAlias($"-{alias}");
        return option;
    }

    public static T WithShortAlias<T>(this T option, params char[] aliases) where T : Option
    {
        foreach (var alias in aliases)
        {
            option.AddAlias($"-{alias}");
        }
        return option;
    }
}