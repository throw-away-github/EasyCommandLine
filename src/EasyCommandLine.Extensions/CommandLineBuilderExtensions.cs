using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Hosting;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;

namespace EasyCommandLine.Extensions;

/// <summary>
/// Extension methods for <see cref="CliConfiguration"/>.
/// </summary>
[UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
public static class CliConfigurationExtensions
{
    public static CliConfiguration AddTitle(this CliConfiguration builder, string title) =>
        builder.AddTitle(title, Color.White);
    
    /// <summary>
    /// Adds a block title to the command line application's help text.
    /// </summary>
    /// <param name="builder">The <see cref="CliConfiguration"/> to add the title to.</param>
    /// <param name="title">The title to add.</param>
    /// <param name="color">The color of the title.</param>
    public static CliConfiguration AddTitle(this CliConfiguration builder, string title, Color color)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            title = builder.RootCommand.Description ?? builder.RootCommand.Name;
        }

        var parseResult = builder.Parse("-h");
        if (parseResult.Action is HelpAction helpAction)
        {
            helpAction.Builder.CustomizeLayout(CustomLayout);
        }

        return builder;
        
        IEnumerable<Func<HelpContext, bool>> CustomLayout(HelpContext _)
        {
            yield return _ =>
            {
                AnsiConsole.Write(new FigletText(title).Color(color)); 
                return true;
            };

            foreach (var section in HelpBuilder.Default.GetLayout().Skip(1))
            {
                yield return section;
            }
        }
    }
    
    public static CliConfiguration UseDependencyInjection(this CliConfiguration builder, Action<IServiceCollection> configureServices)
    {
        return builder.UseDependencyInjection(hostBuilder =>
        {
            hostBuilder.ConfigureServices(configureServices);
        });
    }
    
    /// <summary>
    /// Configure dependency injection for the command line application using the specified <paramref name="configureHost"/> action.
    /// </summary>
    /// <remarks>
    /// This method configures the host using the <see cref="Host.CreateDefaultBuilder(string[])"/>
    /// static method, which loads host and application configuration from environment variables and
    /// and then calls the specified <paramref name="configureHost"/> action.
    /// </remarks>
    /// <param name="builder">The <see cref="CliConfiguration"/> to configure.</param>
    /// <param name="configureHost">The action to configure the host.</param>
    /// <returns>The configured <see cref="CliConfiguration"/>.</returns>
    public static CliConfiguration UseDependencyInjection(this CliConfiguration builder, Action<IHostBuilder> configureHost)
    {
        return builder.UseHost(Host.CreateDefaultBuilder, hostBuilder =>
        {
            hostBuilder.ConfigureServices((_, services) =>
            {
                services.SuppressStatusMessages();
                services.AddSingleton(services);
                services.AddSingleton(AnsiConsole.Console);
            });
            configureHost(hostBuilder);
        });
    }
    
    /// <summary>
    /// Builds a <see cref="CliConfiguration"/> and invokes it with the specified <paramref name="args"/>.
    /// </summary>
    public static Task<int> RunAsync(this CliConfiguration builder, string[] args) =>
        builder.InvokeAsync(args);
}