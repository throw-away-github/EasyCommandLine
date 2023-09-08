using Spectre.Console;
using Spectre.Console.Rendering;

namespace EasyCommandLine.Extensions.Spectre;

public static partial class ConsoleExtensions
{
    /// <summary>
    /// Writes any amount of <see cref="IRenderable"/> to the console wrapped in a <see cref="Columns"/>.
    /// </summary>
    /// <param name="console">The console to write to.</param>
    /// <param name="renderables">The items <see cref="IRenderable"/> to write.</param>
    public static void WriteColumns(this IAnsiConsole console, params IRenderable[] renderables)
    {
        console.Write(new Columns(renderables)
        {
            Expand = false
        });
    }

    /// <inheritdoc cref="WriteColumns(IAnsiConsole,IRenderable[])"/>
    public static void WriteColumnsLine(this IAnsiConsole console, params IRenderable[] renderables)
    {
        console.WriteColumns(renderables);
        console.WriteLine();
    }

    /// <summary>
    /// Writes any amount of <see cref="IRenderable"/> to the console wrapped in a <see cref="Columns"/>.
    /// </summary>
    /// <param name="console">The console to write to.</param>
    /// <param name="expand">Whether or not the <see cref="Columns"/> should expand to the available area.</param>
    /// <param name="renderables">The items <see cref="IRenderable"/> to write.</param>
    public static void WriteColumns(this IAnsiConsole console, bool expand, params IRenderable[] renderables)
    {
        console.Write(new Columns(renderables)
        {
            Expand = expand
        });
    }

    /// <inheritdoc cref="WriteColumns(IAnsiConsole,bool,IRenderable[])"/>
    public static void WriteColumnsLine(this IAnsiConsole console, bool expand, params IRenderable[] renderables)
    {
        console.WriteColumns(expand, renderables);
        console.WriteLine();
    }
}