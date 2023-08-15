using System.CommandLine;
using EasyCommandLine.Core;
using EasyCommandLine.Core.Extensions;

namespace EasyCommandLine.Commands.Hello;

public class HelloCommand : Command<HelloCommandOptions, HelloCommandOptionsHandler>
{
    public HelloCommand() : base("hello", "Say hello to someone")
    {
        
        AddOption(ToOption.WithShortAlias());
        AddOption(ColorOption.WithShortAlias());
    }
    
    private static readonly Option<string?> ToOption = new(
        name: "--to",
        description: "The person to say hello to",
        parseArgument: result =>
        {
            if (result.Tokens.Count > 0) 
                return result.Tokens.Single().Value;
                
            result.ErrorMessage = "--to requires an argument";
            return null;

        })
    {
        Arity = ArgumentArity.ExactlyOne,
        IsRequired = true
    };
    
    private static readonly Option<string> ColorOption = new Option<string>(
            name: "--color", 
            description: "The color to use",
            getDefaultValue: () => "teal")
        .FromAmong("white", "red", "green", "blue", "yellow", "teal", "magenta", "tan");
}