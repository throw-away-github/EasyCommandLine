using Spectre.Console;

namespace EasyCommandLine.Commands.Services;

public class TestService
{
    private readonly IAnsiConsole _console;
    
    public TestService(IAnsiConsole console)
    {
        _console = console;
    }
    
    public async Task Run(CancellationToken cancellationToken = default)
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
                    task1.Increment(1.5);
                    task2.Increment(0.5);
                }
            }).ConfigureAwait(false);
    }
}