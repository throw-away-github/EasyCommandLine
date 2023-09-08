using System.CommandLine;
using JetBrains.Annotations;

namespace EasyCommandLine.Extensions;

/// <summary>
/// Extension methods for <see cref="CliOption"/>.
/// </summary>
[UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
public static class OptionExtensions
{
    /// <summary>
    /// Adds a short alias for the option using the first character of the options' name.
    /// </summary>
    /// <param name="option">The option to add the alias to.</param>
    public static T WithShortAlias<T>(this T option) where T : CliOption
    {
        var name = option.Name.TrimStart('-');
        return name.Length > 1
            ? option.WithShortAlias(name[0]) 
            : option;
    }
    
    /// <summary>
    /// Adds a short alias for the option.
    /// </summary>
    /// <param name="option">The option to add the alias to.</param>
    /// <param name="alias">The alias to add.</param>
    public static T WithShortAlias<T>(this T option, char alias) where T : CliOption
    {
        option.Aliases.Add($"-{alias}");
        return option;
    }

    /// <summary>
    /// Adds short aliases for the option.
    /// </summary>
    /// <param name="option">The option to add the alias to.</param>
    /// <param name="aliases">The aliases to add.</param>
    public static T WithShortAlias<T>(this T option, params char[] aliases) where T : CliOption
    {
        foreach (var alias in aliases)
        {
            option.Aliases.Add($"-{alias}");
        }
        return option;
    }
    
    /// <summary>
    /// Configures the option to accept only the specified values.
    /// </summary>
    /// <remarks>
    /// It is recommended to use <see cref="M:EasyCommandLine.Extensions.OptionExtensions.FromAmong``1(System.CommandLine.CliOption{``0},System.String[])"/>
    /// instead, as it also suggests the values as command line completions.
    /// </remarks>
    /// <param name="option">The option to configure.</param>
    /// <param name="values">The values that are allowed for the option.</param>
    public static CliOption<T> FromAmong<T>(this CliOption<T> option, params T[] values) where T : notnull
    {
        if (values.Length <= 0) 
            return option;
        if (values is string[] stringValues)
        {
            option.AcceptOnlyFromAmong(stringValues);
            return option;
        }
        option.Validators.Add(result =>
        {
            if (result.Tokens.Count <= 0) 
                return;
            var value = result.GetValueOrDefault<T>();
            if (!values.Contains(value)) 
                result.AddError($"The value for {result.Option.Name} must be one of: {string.Join(", ", values)}");
        });
        return option;
    }

    /// <summary>
    /// Configures the option to accept only the specified values, and to suggest them as command line completions.
    /// </summary>
    /// <param name="option">The option to configure.</param>
    /// <param name="values">The values that are allowed for the option.</param>
    public static CliOption<T> FromAmong<T>(this CliOption<T> option, params string[] values) where T : notnull
    {
        if (values.Length <= 0) 
            return option;
        option.AcceptOnlyFromAmong(values);
        return option;
    }
}