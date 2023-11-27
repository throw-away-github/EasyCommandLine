using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EasyCommandLine.Logging;

public class AnsiLoggerOptionsBuilder
{
    public AnsiLoggerOptionsBuilder(IServiceCollection services)
    {
        Services = services;
    }

    /// <summary>
    /// Gets the application services collection.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Allows the logger to automatically serialize objects to JSON using the specified <see cref="JsonSerializerContext"/>.
    /// </summary>
    public AnsiLoggerOptionsBuilder SetJsonContext(JsonSerializerContext context)
    {
        Services.Configure<AnsiLoggerOptions>(opt => opt.Context = context);
        Services.TryAdd(ServiceDescriptor.Singleton(context));
        return this;
    }
}