using EasyCommandLine.Example.Hello;
using EasyCommandLine.Example.Services;
using EasyCommandLine.Example.Test;
using EasyCommandLine.Core;
using EasyCommandLine.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

var builder = CommandLineFactory.CreateDefaultBuilder(
    description: "A simple command line application", 
    new HelloCommand(), new TestCommand());

builder.AddTitle("EzCommand");
builder.UseDependencyInjection(services =>
{
    services.AddSingleton<TestService>();
});

return await builder.RunAsync(args);