using Spectre.Console;

namespace EasyCommandLine.Extensions.Spectre;

public static partial class ConsoleExtensions
{
    /// <summary>
    /// Writes a <see cref="FileSystemInfo"/> to the console using <see cref="TextPath"/>.
    /// </summary>
    /// <param name="console">The console to write to.</param>
    /// <param name="file">The file or directory to write.</param>
    /// <param name="color">The path separator color.</param>
    /// <param name="justify">The text justification. Defaults to <see cref="Justify.Left"/>.</param>
    public static void Write(this IAnsiConsole console, FileSystemInfo file, Color color, Justify justify = Justify.Left)
    {
        console.Write(new TextPath(file.FullName).SeparatorColor(color).RootColor(color).Justify(justify));
    }

    /// <inheritdoc cref="Write(IAnsiConsole,FileSystemInfo,Color,Justify)"/>
    public static void WriteLine(this IAnsiConsole console, FileSystemInfo file, Color color,
        Justify justify = Justify.Left)
    {
        console.Write(file, color, justify);
        console.WriteLine();
    }

    /// <summary>
    /// Writes a <see cref="FileInfo"/> to the console using <see cref="TextPath"/>.
    /// </summary>
    /// <param name="console">The console to write to.</param>
    /// <param name="file">The file to write.</param>
    /// <param name="justify">The text justification. Defaults to <see cref="Justify.Left"/>.</param>
    public static void Write(this IAnsiConsole console, FileSystemInfo file, Justify justify = Justify.Left)
    {
        console.Write(file, Color.Yellow, justify);
    }

    /// <inheritdoc cref="Write(IAnsiConsole,FileSystemInfo,Justify)"/>
    public static void WriteLine(this IAnsiConsole console, FileSystemInfo file, Justify justify = Justify.Left)
    {
        console.Write(file, justify);
        console.WriteLine();
    }
}