using EasyCommandLine.Commands.Hello;
using EasyCommandLine.Commands.Services;
using EasyCommandLine.Core;
using EasyCommandLine.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

var builder = CommandLineFactory.CreateDefaultBuilder(
    description: "A simple command line application", 
    commands: new HelloCommand());

builder.AddTitle("EasyCommandLine");
builder.UseDependencyInjection(services =>
{
    services.AddSingleton<TestService>();
});

return await builder.RunAsync(args);