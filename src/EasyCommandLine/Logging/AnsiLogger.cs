using System.Text.Json;
using EasyCommandLine.Interfaces;
using EasyCommandLine.Extensions.Spectre;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spectre.Console;

namespace EasyCommandLine.Logging;

public class AnsiLogger: IAnsiLogger
{
    private readonly ILogger _logger;
    private readonly IAnsiConsole _console;
    private readonly AnsiLoggerOptions _options;
    
    public AnsiLogger(ILogger logger, IAnsiConsole console, IOptions<AnsiLoggerOptions> options)
    {
        _logger = logger;
        _console = console;
        _options = options.Value;
    }

    public void LogJson<TJson>(TJson obj) where TJson : class
    {
        if (_options.Context is not { } context)
        {
            _logger.LogWarning("No serializer options were configured, calling {Method} will have no effect",
                nameof(LogJson));
            return;
        }

        if (!context.Options.TryGetTypeInfo(typeof(TJson), out var typeInfo))
        {
            _logger.LogWarning(
                "No type info was found for {Type} in the serializer context, add it to {Context} before calling {Method}",
                typeof(TJson).Name, context.GetType().Name, nameof(LogJson));
            return;
        }

        var json = JsonSerializer.Serialize(obj, typeInfo);
        _console.WriteJson(json);
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        _logger.Log(logLevel, eventId, state, exception, formatter);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _logger.IsEnabled(logLevel);
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return _logger.BeginScope(state);
    }
}