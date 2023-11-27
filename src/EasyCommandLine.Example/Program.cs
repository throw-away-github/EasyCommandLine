using EasyCommandLine;
using EasyCommandLine.Example.Hello;
using EasyCommandLine.Example.Services;
using EasyCommandLine.Example.Test;
using Microsoft.Extensions.DependencyInjection;

var builder = CliBuilder.Create(description: "A simple command line application");

builder.AddTitle("EzCommand");
builder.UseAnsiLogging();

builder.AddCommand<HelloCommand>();
builder.AddCommand<TestCommand>();
builder.DefaultCommand<HelloCommand>();

builder.Host.ConfigureServices((_, services) =>
{
    services.AddSingleton<TestService>();
});

return await builder.RunAsync(args);