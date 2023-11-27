using AutoCtor;
using EasyCommandLine.Interfaces;
using Microsoft.Extensions.Logging;

namespace EasyCommandLine.Example.Hello;

[AutoConstruct]
public partial class HelloCommandOptionsHandler : ICommandOptionsHandler<HelloCommandOptions>
{
    private readonly ILogger<HelloCommandOptionsHandler> _logger;

    public Task<int> HandleAsync(HelloCommandOptions options, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Hello [{Color}]{To}[/]!", options.Color.ToLowerInvariant(), options.To);
        return Task.FromResult(0);
    }
}
