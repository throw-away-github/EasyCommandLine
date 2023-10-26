using Microsoft.Extensions.Logging;

namespace EasyCommandLine.Interfaces;

public interface IAnsiLogger<out TCategoryName> : ILogger<TCategoryName>
{
    void LogJson<T>(T obj) where T : class;
}

public interface IAnsiLogger : ILogger
{
    void LogJson<T>(T obj) where T : class;
}