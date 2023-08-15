using EasyCommandLine.Commands.Hello;
using EasyCommandLine.Commands.Services;
using EasyCommandLine.Commands.Test;
using EasyCommandLine.Core;
using EasyCommandLine.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

var builder = CommandLineFactory.CreateDefaultBuilder(
    description: "A simple command line application", 
    new HelloCommand(), new TestCommand());

builder.AddTitle("EasyCommandLine");
builder.UseDependencyInjection(services =>
{
    services.AddSingleton<TestService>();
});

return await builder.RunAsync(args);