using System.CommandLine;
using System.CommandLine.Builder;
using JetBrains.Annotations;

namespace EasyCommandLine.Core;

/// <summary>
/// Factory for creating <see cref="CommandLineBuilder"/> instances.
/// </summary>
[UsedImplicitly]
public static class CommandLineFactory
{
    /// <summary>
    /// Creates a <see cref="CommandLineBuilder"/> with default settings.
    /// </summary>
    /// <param name="rootCommand">The main action that the application performs.</param>
    public static CommandLineBuilder CreateDefaultBuilder(RootCommand rootCommand)
    {
        return new CommandLineBuilder(rootCommand).UseDefaults();
    }
    
    /// <summary>
    /// Creates a <see cref="CommandLineBuilder"/> with default settings.
    /// </summary>
    /// <param name="description">The description of the main action.</param>
    /// <param name="commands">The sub actions that the application performs.</param>
    public static CommandLineBuilder CreateDefaultBuilder(string description, params Command[] commands)
    {
        var rootCommand = new RootCommand(description);
        foreach (var command in commands)
        {
            rootCommand.AddCommand(command);
        }
        return CreateDefaultBuilder(rootCommand);
    }
    
    /// <summary>
    /// Creates a <see cref="CommandLineBuilder"/> with default settings.
    /// </summary>
    /// <param name="rootCommand">The name of the main action.</param>
    /// <param name="commands">The sub actions that the application performs.</param>
    public static CommandLineBuilder CreateDefaultBuilder(RootCommand rootCommand, params Command[] commands)
    {
        foreach (var command in commands)
        {
            rootCommand.AddCommand(command);
        }
        return CreateDefaultBuilder(rootCommand);
    }
}