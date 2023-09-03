using AutoCtor;
using EasyCommandLine.Example.Services;
using EasyCommandLine.Core.Interfaces;
using Spectre.Console;

namespace EasyCommandLine.Example.Hello;

[AutoConstruct]
public partial class HelloCommandOptionsHandler : ICommandOptionsHandler<HelloCommandOptions>
{
    private readonly IAnsiConsole _console;

    public Task<int> HandleAsync(HelloCommandOptions options, CancellationToken cancellationToken)
    {
        _console.MarkupLineInterpolated($"Hello [{options.Color.ToLowerInvariant()}]{options.To}[/]!");
        return Task.FromResult(0);
    }
}
