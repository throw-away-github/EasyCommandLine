using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;

namespace EasyCommandLine.Core.Extensions;

[UsedImplicitly (ImplicitUseTargetFlags.WithMembers)]
public static class CommandLineBuilderExtensions
{
    public static CommandLineBuilder AddTitle(this CommandLineBuilder builder, string title) =>
        builder.AddTitle(title, Color.White);
    
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
    
    public static Task<int> RunAsync(this CommandLineBuilder builder, string[] args) =>
        builder.Build().InvokeAsync(args);
}