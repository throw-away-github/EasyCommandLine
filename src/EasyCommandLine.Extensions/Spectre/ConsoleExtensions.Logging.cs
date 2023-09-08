using System.Globalization;
using System.Reflection.Metadata;
using Spectre.Console;
using Spectre.Console.Advanced;

namespace EasyCommandLine.Extensions.Spectre;

public static partial class ConsoleExtensions
{
    /// <summary>
    /// Escapes markup in the interpolated string, before returning the string.
    /// </summary>
    /// <remarks>
    /// This is copied from Spectre.Console's internal Markup.EscapeInterpolated>.
    /// </remarks>
    /// <param name="provider">The format provider to use.</param>
    /// <param name="value">The interpolated string value to write.</param>
    /// <returns>A string that is safe to use in markup.</returns>
    private static string EscapeInterpolated(IFormatProvider provider, FormattableString value)
    {
        var args = value.GetArguments().Select(arg => arg is string s ? s.EscapeMarkup() : arg).ToArray();
        return string.Format(provider, value.Format, args);
    }
    
    /// <summary>
    /// Wraps <see cref="EscapeInterpolated(IFormatProvider,FormattableString)"/> using the current culture.
    /// </summary>
    /// <param name="value">The interpolated string value to write.</param>
    /// <returns>A string that is safe to use in markup.</returns>
    private static string EscapeInterpolated(FormattableString value)
    {
        return EscapeInterpolated(CultureInfo.CurrentCulture, value);
    }
    
    /// <summary>
    /// Escapes the interpolated string applying markup and style, before returning the string.
    /// </summary>
    /// <remarks>
    /// This is a bit of a hack, preferably we'd simply parse the remaining markup after escaping the interpolated string,
    /// but because Spectre.Console makes nearly everything which returns a string internal (including MarkupInterpolated),
    /// it's the only way to get the string without writing to the console or using reflection.
    /// </remarks>
    /// <param name="console"></param>
    /// <param name="value"></param>
    /// <param name="style"></param>
    /// <returns></returns>
    private static string MarkupInterpolated(IAnsiConsole console, FormattableString value, Style? style = null)
    {
        return console.ToAnsi(new Markup(EscapeInterpolated(value), style));
    }

    private static void Log(this IAnsiConsole console, string emoji, string status, string text, Style style, bool styleMessage)
    {
        var prefix = console.Profile.Capabilities.Unicode ? $"{emoji} {status}: " : $"{status}: ";
        if (styleMessage)
        {
            console.WriteLine($"{prefix}{text}", style);
            return;
        }
        
        console.Write(prefix, style);
        console.WriteLine(text);
    }

    public static void Warning(this IAnsiConsole console, string text, bool styleMessage = true)
    {
        console.Log(Emoji.Known.Warning, "Warning", text, Color.Yellow, styleMessage);
    }

    public static void WarningInterpolated(this IAnsiConsole console, FormattableString text)
    {
        console.Warning(EscapeInterpolated(text), false);
    }

    public static void Error(this IAnsiConsole console, string text, bool style = true)
    {
        console.Log(Emoji.Known.CrossMark, "Error", text, Color.Red, style);
    }

    public static void ErrorInterpolated(this IAnsiConsole console, FormattableString text)
    {
        console.Error(EscapeInterpolated(text), false);
    }

    public static void Success(this IAnsiConsole console, string text, bool style = true)
    {
        console.Log(Emoji.Known.CheckMark, "Success", text, Color.Green, style);
    }

    public static void SuccessInterpolated(this IAnsiConsole console, FormattableString text)
    {
        console.Success(EscapeInterpolated(text), false);
    }

    public static void Info(this IAnsiConsole console, string text, bool style = false)
    {
        console.Log(Emoji.Known.Information, "Info", text, StyleConstants.MemberStyle, style);
    }

    public static void InfoInterpolated(this IAnsiConsole console, FormattableString text)
    {
        console.Info(MarkupInterpolated(console, text), false);
    }
}