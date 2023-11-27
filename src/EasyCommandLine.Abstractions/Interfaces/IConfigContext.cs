namespace EasyCommandLine.Interfaces;

public interface IConfigContext<T> where T : new()
{
    Task<T> GetConfigAsync(CancellationToken token = default);
    Task SaveChangesAsync(CancellationToken token = default);
}