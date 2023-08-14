using EasyCommandLine.Core;
using EasyCommandLine.Core.Interfaces;

namespace EasyCommandLine.Commands.Hello;

public class HelloCommandOptions : ICommandOptions
{
    // Automatic binding with System.CommandLine.NamingConventionBinder
    public string To { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
}