using Spectre.Console;
using Spectre.Console.Advanced;
using Spectre.Console.Rendering;

namespace EasyCommandLine.Extensions.Spectre;

public static partial class ConsoleExtensions
{
    /// <summary>
    /// Writes the specified string value to the console.
    /// </summary>
    /// <param name="console">The console to write to.</param>
    /// <param name="text">The text to write.</param>
    /// <param name="color">The text color.</param>
    /// <param name="decoration">The text decoration.</param>
    public static void Write(this IAnsiConsole console, string text, Color color, Decoration decoration)
    {
        console.Write(text, new Style(color, Color.Default, decoration));
    }

    /// <inheritdoc cref="Write(IAnsiConsole,string,Color,Decoration)"/>
    public static void WriteLine(this IAnsiConsole console, string text, Color color, Decoration decoration)
    {
        console.WriteLine(text, new Style(color, Color.Default, decoration));
    }

    /// <inheritdoc cref="IAnsiConsole.Write(IRenderable)"/>
    public static void WriteLine(this IAnsiConsole console, IRenderable renderable)
    {
        console.Write(renderable);
        console.WriteLine();
    }

    public static string ToEscapedAnsi(this IAnsiConsole console, params IRenderable[] renderables)
    {
        var renderable = renderables.Length == 1 ? renderables[0] : new Columns(renderables);
        return console.ToAnsi(renderable).EscapeMarkup();
    }
}