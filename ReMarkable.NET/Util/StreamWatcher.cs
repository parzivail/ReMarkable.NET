using System;
using System.IO;
using ReMarkable.NET.Unix.Driver.Generic;

namespace ReMarkable.NET.Util
{
    /// <summary>
    ///     Provides a set of methods that allow a stream to be watched for new data to be written, and to have events
    ///     dispatched when new data is available and parsed from the stream
    /// </summary>
    /// <typeparam name="T">The type of struct to read from the stream</typeparam>
    public class StreamWatcher<T> : IDisposable where T : struct
    {
        /// <summary>
        ///     Fired when new data has been parsed from the stream
        /// </summary>
        public event EventHandler<DataAvailableEventArgs<T>> DataAvailable;

        /// <summary>
        ///     The buffer into which data is read
        /// </summary>
        private readonly byte[] _buffer;

        /// <summary>
        ///     The stream from which data is read
        /// </summary>
        private readonly Stream _stream;

        /// <summary>
        ///     Creates a new <see cref="StreamWatcher{T}" />
        /// </summary>
        /// <param name="stream">The stream from which data is read</param>
        /// <param name="bufferSize">The exact size of the type to read</param>
        public StreamWatcher(Stream stream, int bufferSize)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
            _buffer = new byte[bufferSize];
            WatchNext();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _stream?.Dispose();
        }

        /// <summary>
        ///     Fires the DataAvailable event with the specified data arguments
        /// </summary>
        /// <param name="e">The data argument to pass to the event handlers</param>
        protected void OnDataAvailable(DataAvailableEventArgs<T> e)
        {
            DataAvailable?.Invoke(this, e);
        }

        /// <summary>
        ///     Wait for the next data to appear on the stream
        /// </summary>
        protected void WatchNext()
        {
            _stream.BeginRead(_buffer, 0, _buffer.Length, ReadCallback, null);
        }

        /// <summary>
        ///     Process the data when the read buffer is saturated, which is when one struct has been read
        /// </summary>
        /// <param name="ar">The async stream operation result</param>
        private void ReadCallback(IAsyncResult ar)
        {
            var bytesRead = _stream.EndRead(ar);
            if (bytesRead != _buffer.Length)
                throw new InvalidOperationException("Buffer underflow");
            OnDataAvailable(new DataAvailableEventArgs<T>(_buffer.ToStruct<T>()));
            WatchNext();
        }
    }
}