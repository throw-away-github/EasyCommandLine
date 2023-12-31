using AutoCtor;
using EasyCommandLine.Interfaces;

namespace EasyCommandLine.Example.Hello;

[AutoConstruct]
public partial class HelloCommandOptions : ICommandOptions
{
    // Automatic binding with System.CommandLine.NamingConventionBinder
    public string To { get; init; }
    public string Color { get; init; }
}