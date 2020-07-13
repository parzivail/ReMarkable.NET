using System;
using System.IO;
using ReMarkable.NET.Util;

namespace ReMarkable.NET.Unix.Driver.Generic
{
    /// <summary>
    ///     Provides a base set of methods that allow an input device event stream to be parsed and delegated to event handlers
    /// </summary>
    public abstract class UnixInputDriver : IDisposable
    {
        /// <summary>
        ///     The continuous stream parser
        /// </summary>
        private readonly StreamWatcher<EvEvent> _eventWatcher;

        /// <summary>
        ///     The device event stream file to poll for new events
        /// </summary>
        public string Device { get; }

        /// <summary>
        ///     Creates a new <see cref="UnixInputDriver" />
        /// </summary>
        /// <param name="device">The device event stream file to poll for new events</param>
        protected UnixInputDriver(string device)
        {
            Device = device;
            _eventWatcher = new StreamWatcher<EvEvent>(File.OpenRead(Device), 16);
            _eventWatcher.DataAvailable += DataAvailable;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Called when a new event has been parsed from the event stream
        /// </summary>
        /// <param name="sender">The stream from which the event was parsed</param>
        /// <param name="e">The parsed event</param>
        protected abstract void DataAvailable(object sender, DataAvailableEventArgs<EvEvent> e);

        /// <inheritdoc cref="Dispose()" />
        protected virtual void Dispose(bool disposing)
        {
            if (disposing) _eventWatcher?.Dispose();
        }
    }
}