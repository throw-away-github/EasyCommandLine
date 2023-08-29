using System.CommandLine;
using EasyCommandLine.Core;
using EasyCommandLine.Core.Extensions;

namespace EasyCommandLine.Commands.Test;

public class TestCommand : Command<TestCommandOptions, TestCommandsHandler>
{
    public TestCommand() : base("test", "Run a test with the specified number of iterations")
    {
        Add(IterationsOption.WithShortAlias());
    }

    private static readonly CliOption<int?> IterationsOption = new("--iterations")
    {
        Description = "The number of iterations to run minimum 1, maximum 25",
        DefaultValueFactory = _ => 1,
        Validators =
        {
            result =>
            {
                if (!result.Tokens.Any())
                {
                    return;
                }
                if (!int.TryParse(result.Tokens.Single().Value, out var iterations))
                {
                    result.AddError("The value for --iterations must be an integer");
                    return;
                }
                switch (iterations)
                {
                    case < 1:
                        result.AddError("The value for --iterations must be greater than 0");
                        return;
                    case >= 25:
                        result.AddError("The value for --iterations must be 25 or less");
                        return;
                    default:
                        return;
                }
            }
        },
        Arity = ArgumentArity.ZeroOrOne,
    };
}

