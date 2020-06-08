using System;

namespace ReMarkable.NET.Util
{
    public class DataAvailableEventArgs<T> : EventArgs
    {
        public DataAvailableEventArgs(T message)
        {
            Data = message;
        }

        public T Data { get; }
    }
}