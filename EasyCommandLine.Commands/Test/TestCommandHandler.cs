using AutoCtor;
using EasyCommandLine.Commands.Services;
using EasyCommandLine.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace EasyCommandLine.Commands.Test;

[AutoConstruct]
public partial class TestCommandsHandler : ICommandOptionsHandler<TestCommandOptions>
{
    private readonly TestService _testService;
    private readonly ILogger<TestCommandsHandler> _logger;
    
    public async Task<int> HandleAsync(TestCommandOptions options, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Running test service with {Iterations} iterations", options.Iterations);
        try
        {
            await Task.Run(() => _testService.Run(options.Iterations, cancellationToken), cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Test service cancelled");
            return 1;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error running test service");
            return 1;
        }
        return 0;
    }
}