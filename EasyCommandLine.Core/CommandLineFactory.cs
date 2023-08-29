using System.CommandLine;
using System.CommandLine.Help;
using JetBrains.Annotations;

namespace EasyCommandLine.Core;

/// <summary>
/// Factory for creating <see cref="CliConfiguration"/> instances.
/// </summary>
[UsedImplicitly]
public static class CommandLineFactory
{
    /// <summary>
    /// Creates a <see cref="CliConfiguration"/> with default settings.
    /// </summary>
    /// <param name="rootCommand">The main action that the application performs.</param>
    public static CliConfiguration CreateDefaultBuilder(CliRootCommand rootCommand)
    {
        
        return new CliConfiguration(rootCommand);
    }
    
    /// <summary>
    /// Creates a <see cref="CliConfiguration"/> with default settings.
    /// </summary>
    /// <param name="description">The description of the main action.</param>
    /// <param name="commands">The sub actions that the application performs.</param>
    public static CliConfiguration CreateDefaultBuilder(string description, params CliCommand[] commands)
    {
        var rootCommand = new CliRootCommand(description)
        {
            TreatUnmatchedTokensAsErrors = true
        };
        
        foreach (var command in commands)
        {
            rootCommand.Add(command);
        }
        return CreateDefaultBuilder(rootCommand);
    }
    
    /// <summary>
    /// Creates a <see cref="CliConfiguration"/> with default settings.
    /// </summary>
    /// <param name="rootCommand">The name of the main action.</param>
    /// <param name="commands">The sub actions that the application performs.</param>
    public static CliConfiguration CreateDefaultBuilder(CliRootCommand rootCommand, params CliCommand[] commands)
    {
        foreach (var command in commands)
        {
            rootCommand.Add(command);
        }
        return CreateDefaultBuilder(rootCommand);
    }
}