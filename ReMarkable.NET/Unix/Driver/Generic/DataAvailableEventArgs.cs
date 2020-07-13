using System;

namespace ReMarkable.NET.Unix.Driver.Generic
{
    /// <summary>
    ///     Contains data related to data availability events raised by monitored streams
    /// </summary>
    /// <typeparam name="T">The type of data referenced in the event</typeparam>
    public class DataAvailableEventArgs<T> : EventArgs
    {
        /// <summary>
        ///     The data referenced by the event
        /// </summary>
        public T Data { get; }

        /// <summary>
        ///     Creates a new <see cref="DataAvailableEventArgs{T}" />
        /// </summary>
        /// <param name="data">The data referenced by the event</param>
        public DataAvailableEventArgs(T data)
        {
            Data = data;
        }
    }
}