using System.CommandLine;
using System.CommandLine.Builder;

namespace EasyCommandLine.Core;

public static class CommandLineFactory
{
    public static CommandLineBuilder CreateDefaultBuilder(RootCommand rootCommand)
    {
        var builder = new CommandLineBuilder(rootCommand).UseDefaults();
        return builder;
    }
    
    public static CommandLineBuilder CreateDefaultBuilder(string description, params Command[] commands)
    {
        var rootCommand = new RootCommand(description);
        foreach (var command in commands)
        {
            rootCommand.AddCommand(command);
        }
        return CreateDefaultBuilder(rootCommand);
    }
}