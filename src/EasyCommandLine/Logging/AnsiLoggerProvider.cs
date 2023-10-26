using System.Collections.Concurrent;
using EasyCommandLine.Interfaces;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Vertical.SpectreLogger;

namespace EasyCommandLine.Logging;

public sealed class AnsiLoggerProvider: IAnsiLoggerProvider
{
    private readonly IOptions<AnsiLoggerOptions> _optionsProvider;
    private readonly IAnsiConsole _console;
    private readonly SpectreLoggerProvider _spectreLoggerProvider;
        
    private readonly ConcurrentDictionary<string, IAnsiLogger> _cachedLoggers = new();

    public AnsiLoggerProvider(IOptions<AnsiLoggerOptions> optionsProvider, IAnsiConsole console, SpectreLoggerProvider spectreLoggerProvider)
    {
        _optionsProvider = optionsProvider;
        _console = console;
        _spectreLoggerProvider = spectreLoggerProvider;
    }

    public IAnsiLogger CreateAnsiLogger(string categoryName)
    {
        return _cachedLoggers.GetOrAdd(categoryName, name =>
            new AnsiLogger(_spectreLoggerProvider.CreateLogger(name), _console, _optionsProvider));
    }

    public void Dispose()
    {
        _spectreLoggerProvider.Dispose();
    }
}