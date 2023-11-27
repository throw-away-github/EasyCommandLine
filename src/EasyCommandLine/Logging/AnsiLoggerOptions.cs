using System.Text.Json.Serialization;

namespace EasyCommandLine.Logging;

public class AnsiLoggerOptions
{
    public JsonSerializerContext? Context { get; set; }
}