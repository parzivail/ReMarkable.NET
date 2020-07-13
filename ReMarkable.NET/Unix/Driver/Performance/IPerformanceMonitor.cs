namespace ReMarkable.NET.Unix.Driver.Performance
{
    /// <summary>
    ///     Provides an interface through which the system performance can be profiled
    /// </summary>
    public interface IPerformanceMonitor
    {
        /// <summary>
        ///     Gets the number of total logical cores in the device
        /// </summary>
        int NumberOfCores { get; }

        /// <summary>
        ///     Gets the number of physical processors in the device
        /// </summary>
        int NumberOfProcessors { get; }

        /// <summary>
        ///     Gets the total amount of memory in the device, in bytes
        /// </summary>
        long TotalMemory { get; }

        /// <summary>
        ///     Gets the total amount of swap in the device, in bytes
        /// </summary>
        long TotalSwap { get; }

        /// <summary>
        ///     Gets the total amount of free memory
        /// </summary>
        /// <returns>The amount of free memory, in bytes</returns>
        long GetFreeMemory();

        /// <summary>
        ///     Gets the total amount of free swap
        /// </summary>
        /// <returns>The amount of free swap, in bytes</returns>
        long GetFreeSwap();

        /// <summary>
        ///     Lists all of the available network adapters
        /// </summary>
        /// <returns></returns>
        string[] GetNetworkAdapters();

        /// <summary>
        ///     Gets the instantaneous network download speed
        /// </summary>
        /// <param name="adapter">The adapter to query</param>
        /// <returns>The speed in bytes/second</returns>
        long GetNetworkRxSpeed(string adapter);

        /// <summary>
        ///     Gets the total network download utilization
        /// </summary>
        /// <param name="adapter">The adapter to query</param>
        /// <returns>The utilization in bytes</returns>
        long GetNetworkRxTotal(string adapter);

        /// <summary>
        ///     Gets the instantaneous network upload speed
        /// </summary>
        /// <param name="adapter">The adapter to query</param>
        /// <returns>The speed in bytes/second</returns>
        long GetNetworkTxSpeed(string adapter);

        /// <summary>
        ///     Gets the total network upload utilization
        /// </summary>
        /// <param name="adapter">The adapter to query</param>
        /// <returns>The utilization in bytes</returns>
        long GetNetworkTxTotal(string adapter);

        /// <summary>
        ///     Gets the total processor utilization
        /// </summary>
        /// <returns>The percentage utilization, from 0-1</returns>
        float GetProcessorTime();

        /// <summary>
        ///     Gets a specific processor's utilization
        /// </summary>
        /// <param name="processor">The processor to query</param>
        /// <returns>The percentage utilization, from 0-1</returns>
        float GetProcessorTime(int processor);

        /// <summary>
        ///     Gets a specific core's utilization
        /// </summary>
        /// <param name="processor">The processor to query</param>
        /// <param name="core">The core to query</param>
        /// <returns>The percentage utilization, from 0-1</returns>
        float GetProcessorTime(int processor, int core);
    }
}