using EasyCommandLine.Interfaces;
using Microsoft.Extensions.Logging;

namespace EasyCommandLine.Logging;

public class AnsiLogger<T> : IAnsiLogger<T>
{
    private readonly IAnsiLogger _logger;

    public AnsiLogger(IAnsiLoggerProvider loggerProvider)
    {
        _logger = loggerProvider.CreateAnsiLogger(GetCategoryName());
    }

    public void LogJson<TJson>(TJson obj) where TJson : class
    {
        _logger.LogJson(obj);
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

    private static string GetCategoryName()
    {
        return TypeNameHelper.GetTypeDisplayName(typeof(T), includeGenericParameters: false, nestedTypeDelimiter: '.');
    }
}