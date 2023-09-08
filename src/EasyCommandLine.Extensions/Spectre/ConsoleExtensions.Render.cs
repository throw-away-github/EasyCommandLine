using Spectre.Console;

namespace EasyCommandLine.Extensions.Spectre;

public static partial class ConsoleExtensions
{
    /// <summary>
    /// Renders a <see cref="string"/> to the console using <see cref="Markup"/>.
    /// </summary>
    /// <param name="console">The console to write to.</param>
    /// <param name="text">The text to write.</param>
    /// <param name="style">The text color.</param>
    /// <param name="overflow">The text overflow. Defaults to <see cref="Overflow.Ellipsis"/>.</param>
    /// <param name="justify">The text justification. Defaults to <see cref="Justify.Left"/>.</param>
    public static void Render(this IAnsiConsole console, string text, Style style,
        Overflow overflow = Overflow.Ellipsis,
        Justify justify = Justify.Left)
    {
        console.Write(new Markup(text, style).Overflow(overflow).Justify(justify));
    }

    /// <inheritdoc cref="Render(IAnsiConsole,string,Style,Overflow,Justify)"/>
    public static void RenderLine(this IAnsiConsole console, string text, Style style,
        Overflow overflow = Overflow.Ellipsis,
        Justify justify = Justify.Left)
    {
        console.Render(text, style, overflow, justify);
        console.WriteLine();
    }
}