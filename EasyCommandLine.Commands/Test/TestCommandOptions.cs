using AutoCtor;
using EasyCommandLine.Core.Interfaces;

namespace EasyCommandLine.Commands.Test;

[AutoConstruct]
public partial class TestCommandOptions : ICommandOptions
{
    public int Iterations { get; init; }
}