using EasyCommandLine.Commands.Hello;
using EasyCommandLine.Commands.Services;
using EasyCommandLine.Core.Interfaces;
using Spectre.Console;

namespace EasyCommandLine.Commands.Hello;


public class HelloCommandOptionsHandler : ICommandOptionsHandler<HelloCommandOptions>
{
    private readonly IAnsiConsole _console;
    private readonly TestService _testService;
    
    public HelloCommandOptionsHandler(IAnsiConsole console, TestService testService)
    {
        _console = console;
        _testService = testService;
    }

    public async Task<int> HandleAsync(HelloCommandOptions options, CancellationToken cancellationToken)
    {
        _console.MarkupLineInterpolated($"Hello [{options.Color.ToLowerInvariant()}]{options.To}[/]!");
        await Task.Run(() => _testService.Run(cancellationToken), cancellationToken);
        return 0;
    }
}
