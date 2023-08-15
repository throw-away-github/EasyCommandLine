using AutoCtor;
using EasyCommandLine.Commands.Services;
using EasyCommandLine.Core.Interfaces;
using Spectre.Console;

namespace EasyCommandLine.Commands.Hello;

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
