using System.CommandLine;
using EasyCommandLine.Core;
using EasyCommandLine.Core.Extensions;

namespace EasyCommandLine.Commands.Test;

public class TestCommand : Command<TestCommandOptions, TestCommandsHandler>
{
    public TestCommand() : base("test", "Run a test with the specified number of iterations")
    {

        AddOption(IterationsOption.WithShortAlias());
    }

    private static readonly Option<int?> IterationsOption = new(
        name: "--iterations",
        description: "The number of iterations to run minimum 1, maximum 25",
        isDefault: true,
        parseArgument: result =>
        {
            if (!result.Tokens.Any())
            {
                return 1;
            }
            if (!int.TryParse(result.Tokens.Single().Value, out var iterations))
            {
                result.ErrorMessage = "The value for --iterations must be an integer";
                return null;
            }
            if (iterations < 1)
            {
                result.ErrorMessage = "The value for --iterations must be greater than 0";
                return null;
            }
            if (iterations >= 25)
            {
                result.ErrorMessage = "The value for --iterations must be 25 or less";
                return null;
            }
            return iterations;
        })
    {
        Arity = ArgumentArity.ZeroOrOne,
    };
}

