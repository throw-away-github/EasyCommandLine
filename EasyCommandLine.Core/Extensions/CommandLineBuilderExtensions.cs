using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;

namespace EasyCommandLine.Core.Extensions;

/// <summary>
/// Extension methods for <see cref="CommandLineBuilder"/>.
/// </summary>
[UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
public static class CommandLineBuilderExtensions
{
    public static CommandLineBuilder AddTitle(this CommandLineBuilder builder, string title) =>
        builder.AddTitle(title, Color.White);
    
    /// <summary>
    /// Adds a block title to the command line application's help text.
    /// </summary>
    /// <param name="builder">The <see cref="CommandLineBuilder"/> to add the title to.</param>
    /// <param name="title">The title to add.</param>
    /// <param name="color">The color of the title.</param>
    public static CommandLineBuilder AddTitle(this CommandLineBuilder builder, string title, Color color)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            title = builder.Command.Description ?? builder.Command.Name;
        }
        return builder.UseHelp(helpContext =>
        {
            helpContext.HelpBuilder.CustomizeLayout(
                _ => HelpBuilder.Default
                    .GetLayout()
                    .Skip(1) // Skip the default command description section.
                    .Prepend(
                        _ => AnsiConsole.Write(new FigletText(title).Color(color))));
        });
    }
    
    public static CommandLineBuilder UseDependencyInjection(this CommandLineBuilder builder, Action<IServiceCollection> configureServices)
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
    /// <param name="builder">The <see cref="CommandLineBuilder"/> to configure.</param>
    /// <param name="configureHost">The action to configure the host.</param>
    /// <returns>The configured <see cref="CommandLineBuilder"/>.</returns>
    public static CommandLineBuilder UseDependencyInjection(this CommandLineBuilder builder, Action<IHostBuilder> configureHost)
    {
        return builder.UseHost(Host.CreateDefaultBuilder, hostBuilder =>
        {
            hostBuilder.ConfigureServices((_, services) =>
            {
                services.SuppressStatusMessages();
                services.AddSingleton(AnsiConsole.Console);
            });
            configureHost(hostBuilder);
        });
    }
    
    /// <summary>
    /// Builds a <see cref="CommandLineBuilder"/> and invokes it with the specified <paramref name="args"/>.
    /// </summary>
    public static Task<int> RunAsync(this CommandLineBuilder builder, string[] args) =>
        builder.Build().InvokeAsync(args);
}