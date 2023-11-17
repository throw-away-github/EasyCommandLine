using System.CommandLine;
using System.CommandLine.Hosting;
using System.Text.Json.Serialization;
using EasyCommandLine.Extensions;
using EasyCommandLine.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace EasyCommandLine;

public class CliBuilder
{
    private readonly CliConfiguration _configuration;
    private RunOption _runOption = RunOption.None;
    private string _noArgsErrorMessage;
    private string _title;
    private Color _titleColor;
    private Func<IEnumerable<CliSymbol>, CliSymbol?> _getDefaultCommand;

    public CliBuilder(CliConfiguration configuration)
    {
        _configuration = configuration;
        _title = _configuration.RootCommand.Name;
        _titleColor = Color.White;
        _getDefaultCommand = null!;
        _noArgsErrorMessage = "A command is required.";
        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder();
    }
    
    public static CliBuilder Create(CliConfiguration configuration)
    {
        return new CliBuilder(configuration);
    }
    
    public static CliBuilder Create(string description = "")
    {
        return Create(CommandLineFactory.CreateDefaultBuilder(description));
    }
    
    public static CliBuilder Create(string description, params CliCommand[] commands)
    {
        return Create(CommandLineFactory.CreateDefaultBuilder(description, commands));
    }

    /// <inheritdoc cref="Microsoft.Extensions.Hosting.IHostBuilder"/>
    public IHostBuilder Host { get; }

    /// <inheritdoc cref="M:System.CommandLine.CliCommand.Add(System.CommandLine.CliCommand)" />
    public CliBuilder AddCommand(CliCommand command)
    {
        _configuration.RootCommand.Add(command);
        return this;
    }

    /// <summary>
    /// Adds a new command to the command line application.
    /// </summary>
    /// <param name="configure"></param>
    /// <typeparam name="T">The type of the command to add.</typeparam>
    public CliBuilder AddCommand<T>(Action<T>? configure = null) where T : CliCommand, new()
    {
        var command = new T();
        configure?.Invoke(command);
        return AddCommand(command);
    }

    /// <summary>
    /// Shows the generated help text when no arguments are passed to the application.
    /// </summary>
    public CliBuilder ShowHelpForNoArgs() => Set(RunOption.ShowHelpForNoArgs);

    /// <summary>
    /// Disables showing the generated help text when no arguments are passed to the application.
    /// </summary>
    public CliBuilder HideHelpForNoArgs() => Unset(RunOption.ShowHelpForNoArgs);

    /// <inheritdoc cref="ShowHelpForNoArgs()"/>
    /// <param name="message">The error message to display.</param>
    public CliBuilder ShowHelpForNoArgs(string message)
    {
        _noArgsErrorMessage = message;
        return ShowHelpForNoArgs();
    }

    /// <summary>
    /// Pass options/arguments to the first command when no command is explicitly specified by the user
    /// </summary>
    public CliBuilder DefaultCommand()
    {
        return DefaultCommand(cliSymbols => cliSymbols.FirstOrDefault());
    }

    /// <inheritdoc cref="DefaultCommand()"/>
    /// <typeparam name="T">The type of the command to use as default.</typeparam>
    public CliBuilder DefaultCommand<T>() where T : CliCommand, new()
    {
        return DefaultCommand(cliSymbols => cliSymbols.OfType<T>().FirstOrDefault());
    }

    /// <inheritdoc cref="DefaultCommand()"/>
    /// <param name="defaultCommand">A function that returns the command to use as default from the root commands children.</param>
    public CliBuilder DefaultCommand(Func<IEnumerable<CliSymbol>, CliSymbol?> defaultCommand)
    {
        _getDefaultCommand = defaultCommand;
        return Set(RunOption.DefaultCommand);
    }

    /// <inheritdoc cref="CliConfigurationExtensions.AddTitle(CliConfiguration,System.String,Spectre.Console.Color)"/>
    public CliBuilder AddTitle(string? title = null, Color? color = null)
    {
        if (title is not null)
            _title = title;
        if (color.HasValue)
            _titleColor = color.Value;
        return Set(RunOption.Title);
    }
    
    /// <summary>
    /// Allows the logger/configuration to automatically serialize objects to JSON using the specified <see cref="JsonSerializerContext"/>.
    /// </summary>
    public CliBuilder SetJsonContext(JsonSerializerContext jsonSerializerContext)
    {
        Host.ConfigureServices(services =>
        {
            services.TryAdd(ServiceDescriptor.Singleton(jsonSerializerContext));
            services.Configure<AnsiLoggerOptions>(opt => opt.Context = jsonSerializerContext);
        });
        return this;
    }
    
    /// <summary>
    /// Adds a default logger to the host that writes to the console using ANSI escape codes.
    /// </summary>
    public CliBuilder UseAnsiLogging()
    {
        Host.ConfigureLogging(builder => builder.AddAnsiLogger());
        return this;
    }

    public CliBuilder UseDefaults()
    {
        return ShowHelpForNoArgs()
            .DefaultCommand()
            .AddTitle()
            .UseAnsiLogging();
    }

    /// <inheritdoc cref="CliConfigurationExtensions.RunAsync(CliConfiguration,System.String[])"/>
    public Task<int> RunAsync(string[] args)
    {
        Host.ConfigureServices(services =>
        {
            services.Configure<InvocationLifetimeOptions>(options => 
                options.SuppressStatusMessages = true);
            services.AddSingleton(services);
            services.AddSingleton(AnsiConsole.Console);
        });

        _configuration.UseHost(filteredArgs => Host
            .ConfigureHostConfiguration(config => config.AddCommandLine(filteredArgs))
            .ConfigureAppConfiguration((_, config) => config.AddCommandLine(filteredArgs)));

        var rootCommand = _configuration.RootCommand;

        if (IsOptionSet(RunOption.ShowHelpForNoArgs))
        {
            rootCommand.TreatUnmatchedTokensAsErrors = false;
            rootCommand.Validators.Add(result =>
            {
                if (!result.Children.Any())
                    result.AddError(_noArgsErrorMessage);
            });
        }

        if (IsOptionSet(RunOption.DefaultCommand)
            && args.Length > 0
            && !_configuration.Parse(args).RootCommandResult.Children.Any()
            && _getDefaultCommand(rootCommand.Children) is { } command)
        {
            args = args.Prepend(command.Name).ToArray();
        }

        if (IsOptionSet(RunOption.Title))
        {
            _configuration.AddTitle(_title, _titleColor);
        }

        return RunSafeAsync();

        async Task<int> RunSafeAsync()
        {
            try
            {
                return await _configuration.RunAsync(args);
            }
            catch (Exception)
            {
                var logger = GetLogger();
                logger.LogError("Unhandled exception");
                return 1;
            }
        }

        ILogger GetLogger()
        {
            ILoggerFactory logFactory;
            try
            {
                logFactory = Host.Build().Services.GetRequiredService<ILoggerFactory>();
            }
            catch (Exception)
            {
                logFactory = LoggerFactory.Create(builder => builder.AddAnsiLogger());
            }

            return logFactory.CreateLogger(_title);
        }
    }

    private CliBuilder Set(RunOption option)
    {
        _runOption |= option;
        return this;
    }

    private CliBuilder Unset(RunOption option)
    {
        _runOption &= ~option;
        return this;
    }

    private bool IsOptionSet(RunOption option)
    {
        return option == RunOption.None
            ? _runOption == RunOption.None
            : (_runOption & option) == option;
    }
    
    [Flags]
    private enum RunOption
    {
        None = 0x0,
        ShowHelpForNoArgs = 0x2,
        DefaultCommand = 0x4,
        Title = 0x8
    }
}