using Spectre.Console;
using Spectre.Console.Json;

namespace EasyCommandLine.Extensions.Spectre;

public static partial class ConsoleExtensions
{
    /// <summary>
    /// Writes a JSON string to the console.
    /// </summary>
    /// <param name="console">The console to write to.</param>
    /// <param name="json">The JSON string to write.</param>
    public static void WriteJson(this IAnsiConsole console, string json)
    {
        console.Write(new JsonText(json)
            .StringStyle(StyleConstants.StringStyle)
            .NumberStyle(StyleConstants.NumberStyle)
            .BooleanStyle(StyleConstants.KeywordStyle)
            .NullStyle(StyleConstants.KeywordStyle)
            .MemberStyle(StyleConstants.MemberStyle)
            .BracketStyle(StyleConstants.BracketStyle)
            .BracesStyle(StyleConstants.BracketStyle));
        console.WriteLine();
    }
}