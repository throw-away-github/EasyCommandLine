using EasyCommandLine;
using EasyCommandLine.Example.Hello;
using EasyCommandLine.Example.Services;
using EasyCommandLine.Example.Test;
using EasyCommandLine.Extensions;
using EasyCommandLine.Extensions.Spectre;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = CommandLineFactory.CreateDefaultBuilder(
    description: "A simple command line application", 
    new HelloCommand(), new TestCommand());

builder.AddTitle("EzCommand");
builder.UseDependencyInjection(hostBuilder =>
{
    hostBuilder.ConfigureServices(services =>
    {
        services.AddSingleton<TestService>();
    });
    hostBuilder.ConfigureLogging(logging => logging.ConfigurePrettyConsole());
});

return await builder.RunAsync(args);