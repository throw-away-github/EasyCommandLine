using AutoCtor;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace EasyCommandLine.Commands.Services;

[AutoConstruct]
public partial class TestService
{
    private readonly IAnsiConsole _console;
    
    public async Task Run(int iterations, CancellationToken cancellationToken = default)
    {
        // show a animated progress bar
        var progress = _console.Progress()
            .AutoClear(false)
            .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new RemainingTimeColumn(), new SpinnerColumn());

        await progress
            .StartAsync(async ctx =>
            {
                // Define tasks
                var task1 = ctx.AddTask("[green]Reticulating splines[/]");
                var task2 = ctx.AddTask("[green]Folding space[/]");

                while (!ctx.IsFinished)
                {
                    // Simulate some work
                    await Task.Delay(250, cancellationToken);

                    // Increment
                    var increment = 25 / (double)iterations;
                    task1.Increment(increment);
                    task2.Increment(increment/2);
                }
            }).ConfigureAwait(false);
    }
}