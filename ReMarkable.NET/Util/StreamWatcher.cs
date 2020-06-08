using System;
using System.IO;

namespace ReMarkable.NET.Util
{
    class StreamWatcher<T> : IDisposable where T : struct
    {
        private readonly Stream _stream;
        private readonly byte[] _buffer;

        public event EventHandler<DataAvailableEventArgs<T>> DataAvailable;

        public StreamWatcher(Stream stream, int bufferSize)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
            _buffer = new byte[bufferSize];
            WatchNext();
        }

        protected void OnDataAvailable(DataAvailableEventArgs<T> e)
        {
            DataAvailable?.Invoke(this, e);
        }

        protected void WatchNext()
        {
            _stream.BeginRead(_buffer, 0, _buffer.Length, ReadCallback, null);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            var bytesRead = _stream.EndRead(ar);
            if (bytesRead != _buffer.Length)
                throw new InvalidOperationException("Buffer underflow");
            OnDataAvailable(new DataAvailableEventArgs<T>(_buffer.ToStruct<T>()));
            WatchNext();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _stream?.Dispose();
        }
    }
}
