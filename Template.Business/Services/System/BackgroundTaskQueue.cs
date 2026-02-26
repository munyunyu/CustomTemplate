using System.Threading.Channels;
using Template.Business.Interfaces.System;

namespace Template.Business.Services.System
{
    public class BackgroundTaskQueue<T> : IBackgroundTaskQueue<T>
    {
        private readonly Channel<T> _channel;

        public BackgroundTaskQueue(int capacity = 100)
        {
            var options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _channel = Channel.CreateBounded<T>(options);
        }

        public ValueTask EnqueueAsync(T item, CancellationToken cancellationToken = default)
            => _channel.Writer.WriteAsync(item, cancellationToken);

        public IAsyncEnumerable<T> DequeueAllAsync(CancellationToken cancellationToken)
            => _channel.Reader.ReadAllAsync(cancellationToken);
    }
}
