using Microsoft.Extensions.Logging;

namespace EasyCommandLine.Interfaces;

public interface IAnsiLoggerProvider : ILoggerProvider
{
    /// <summary>
    /// Creates a new <see cref="ILogger"/> instance.
    /// </summary>
    /// <param name="categoryName">The category name for messages produced by the logger.</param>
    /// <returns>The instance of <see cref="ILogger"/> that was created.</returns>
    IAnsiLogger CreateAnsiLogger(string categoryName);
    
    ILogger ILoggerProvider.CreateLogger(string categoryName)
    {
        return CreateAnsiLogger(categoryName);
    }
}