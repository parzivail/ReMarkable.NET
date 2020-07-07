using System;
using System.IO;
using ReMarkable.NET.Unix.Driver.Generic;
using ReMarkable.NET.Util;

namespace ReMarkable.NET.Unix.Driver
{
    public abstract class UnixInputDriver : IDisposable
    {
        private readonly StreamWatcher<EvEvent> _eventWatcher;

        protected UnixInputDriver(string device)
        {
            Device = device;
            _eventWatcher = new StreamWatcher<EvEvent>(File.OpenRead(Device), 16);
            _eventWatcher.DataAvailable += DataAvailable;
        }

        public string Device { get; }

        protected abstract void DataAvailable(object sender, DataAvailableEventArgs<EvEvent> e);

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) _eventWatcher?.Dispose();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}