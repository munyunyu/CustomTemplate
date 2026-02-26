namespace Template.Business.Interfaces.System
{
    public interface IBackgroundTaskQueue<T>
    {
        ValueTask EnqueueAsync(T item, CancellationToken cancellationToken = default);
        IAsyncEnumerable<T> DequeueAllAsync(CancellationToken cancellationToken);
    }
}
