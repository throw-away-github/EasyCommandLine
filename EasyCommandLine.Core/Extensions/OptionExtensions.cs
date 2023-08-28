using System.CommandLine;
using JetBrains.Annotations;

namespace EasyCommandLine.Core.Extensions;

/// <summary>
/// Extension methods for <see cref="Option"/>.
/// </summary>
[UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
public static class OptionExtensions
{
    /// <summary>
    /// Adds a short alias for the option using the first character of the options' name.
    /// </summary>
    /// <param name="option">The option to add the alias to.</param>
    public static T WithShortAlias<T>(this T option) where T : Option
    {
        return option.Name.Length > 1
            ? option.WithShortAlias(option.Name[0]) 
            : option;
    }
    
    /// <summary>
    /// Adds a short alias for the option.
    /// </summary>
    /// <param name="option">The option to add the alias to.</param>
    /// <param name="alias">The alias to add.</param>
    public static T WithShortAlias<T>(this T option, char alias) where T : Option
    {
        option.AddAlias($"-{alias}");
        return option;
    }

    /// <summary>
    /// Adds short aliases for the option.
    /// </summary>
    /// <param name="option">The option to add the alias to.</param>
    /// <param name="aliases">The aliases to add.</param>
    public static T WithShortAlias<T>(this T option, params char[] aliases) where T : Option
    {
        foreach (var alias in aliases)
        {
            option.AddAlias($"-{alias}");
        }
        return option;
    }
}