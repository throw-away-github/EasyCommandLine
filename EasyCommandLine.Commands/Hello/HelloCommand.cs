using System.CommandLine;
using EasyCommandLine.Commands.Hello;
using EasyCommandLine.Core;

namespace EasyCommandLine.Commands.Hello;


public class HelloCommand : Command<HelloCommandOptions, HelloCommandOptionsHandler>
{
    public HelloCommand() : base("hello", "Say hello to someone")
    {
        AddOption(new Option<string?>(
            name: "--to", 
            description: "The person to say hello to",
            parseArgument: result =>
            {
                if (result.Tokens.Count == 0)
                {
                    result.ErrorMessage = "--to requires an argument";
                    return null;
                }
                return result.Tokens.Single().Value;
            })
        {
            Arity = ArgumentArity.ExactlyOne,
            IsRequired = true
        });

        AddOption(new Option<string>(
            name: "--color", 
            description: "The color to use",
            getDefaultValue: () => "teal")
            .FromAmong("white", "red", "green", "blue", "yellow", "teal", "magenta", "tan"));
    }
}