using System.CommandLine;
using System.CommandLine.Parsing;
using EasyCommandLine.Extensions;

namespace EasyCommandLine.Example.Hello;

public class HelloCommand : Command<HelloCommandOptions, HelloCommandOptionsHandler>
{
    public HelloCommand() : base("hello", "Say hello to someone")
    {
        Add(new CliOption<string>(name: "--to")
        {
            Arity = ArgumentArity.ExactlyOne,
            Aliases = { "-t" },
            Required = true,
            Description = "The person to say hello to",
            Validators =
            {
                new Action<OptionResult>(result =>
                {
                    if (result.Tokens.Count <= 0)
                        result.AddError("The --to option requires a value");
                })
            }
        });
        Add(new CliOption<string>(name: "--color") 
            {
                Aliases = { "-c" },
                Description = "The color to use for the greeting",
                DefaultValueFactory = _ => "teal",
            }
            .FromAmong("white", "red", "green", "blue", "yellow", "teal", "magenta", "tan"));
    }
}