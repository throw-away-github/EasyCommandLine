using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using EasyCommandLine.Interfaces;
using Microsoft.Extensions.Logging;

namespace EasyCommandLine.Config;

public class ConfigContext<T> : IConfigContext<T> where T : new()
{
    private readonly ILogger<ConfigContext<T>> _logger;
    private readonly JsonTypeInfo<T> _typeInfo;
    private readonly FileInfo _file;
    private T? _config;

    public ConfigContext(ILogger<ConfigContext<T>> logger, JsonTypeInfo<T> typeInfo, FileInfo file)
    {
        _logger = logger;
        _typeInfo = typeInfo;
        _file = file;
    }

    public async Task<T> GetConfigAsync(CancellationToken token = default)
    {
        try
        {
            var config = await GetConfigCoreAsync(token).ConfigureAwait(false);
            _config ??= config;
            return config;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to read config file, backing up and creating fresh default");
            return await CreateConfig(token).ConfigureAwait(false);
        }
    }

    private ValueTask<T> GetConfigCoreAsync(CancellationToken token = default)
    {
        return _config is not null
            ? new ValueTask<T>(_config)
            : Await();

        async ValueTask<T> Await()
        {
            if (!_file.Exists)
            {
                _logger.LogInformation("Config file does not exist, creating new config at {Path}", _file.FullName);
                await CreateConfig(token);
            }

            await using var read = _file.OpenRead();
            var result = await JsonSerializer.DeserializeAsync(read, _typeInfo, token);
            return result ?? throw new NullReferenceException("Config is null");
        }
    }

    public async Task SaveChangesAsync(CancellationToken token = default)
    {
        if (_config is null)
        {
            _logger.LogWarning("Attempted to save config before loading");
            return;
        }
        
        await using var write = _file.OpenWrite();
        await JsonSerializer.SerializeAsync(write, _config, _typeInfo, token);
    }

    private async Task<T> CreateConfig(CancellationToken token = default)
    {
        var config = new T();

        if (_file.Exists)
        {
            var backup = Path.Combine(_file.DirectoryName!, $"{_file.Name}-{DateTime.UtcNow:M-d-yyyy-HH-mm-ss}.bak");
            _logger.LogWarning("Config file already exists, backing up to {Path}", backup);
            File.Move(_file.FullName, backup);
        }

        if (!_file.Directory!.Exists)
            _file.Directory.Create();
        
        await using var write = _file.OpenWrite();
        await JsonSerializer.SerializeAsync(write, config, _typeInfo, token);

        _config = config;
        return config;
    }
}