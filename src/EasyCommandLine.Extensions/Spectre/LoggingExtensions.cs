using System.Text;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Vertical.SpectreLogger;
using Vertical.SpectreLogger.Options;

namespace EasyCommandLine.Extensions.Spectre;

public static class LoggingExtensions
{
    public static void ConfigurePrettyConsole(this ILoggingBuilder log,
        Action<SpectreLoggingBuilder>? configureBuilder = null)
    {
        log.ClearProviders().AddDebug();
        log.AddSpectreConsole(config =>
        {
            config.ConfigureProfiles(profile => profile.OutputTemplate = OutputTemplate(profile.LogLevel));
            configureBuilder?.Invoke(config);
        });
    }
    
    private static string OutputTemplate(LogLevel level, string text = "{Message}{NewLine}{Exception}")
    {
        var sb = new StringBuilder();
        sb.Append(level switch {
            LogLevel.Trace => "[grey35]",
            LogLevel.Debug => "[grey46]",
            LogLevel.Information => $"[{StyleConstants.MemberStyle.ToMarkup()}]",
            LogLevel.Warning => "[gold3_1]",
            LogLevel.Error => "[red1]",
            LogLevel.Critical => "[white on red1]",
            _ => "[green3_1]"
        });
        
        if (AnsiConsole.Profile.Capabilities.Unicode)
        {
            sb.Append(level switch
            {
                LogLevel.Trace => Emoji.Known.Link,
                LogLevel.Debug => Emoji.Known.Bug,
                LogLevel.Information => Emoji.Known.Information,
                LogLevel.Warning => Emoji.Known.Warning,
                LogLevel.Error => Emoji.Known.CrossMark,
                LogLevel.Critical => Emoji.Known.Firecracker,
                _ => Emoji.Known.Information
            });
            sb.Append(' ');
        }
        
        sb.Append(level switch {
            LogLevel.Trace => "Trace",
            LogLevel.Debug => "Debug",
            LogLevel.Information => "Info",
            LogLevel.Warning => "Warn",
            LogLevel.Error => "Error",
            LogLevel.Critical => "Critical",
            _ => "Info"
        });
        
        sb.Append(": [/]");
        sb.Append(text);
        
        return sb.ToString();
    }
}