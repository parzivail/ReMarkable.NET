using ReMarkable.NET.Unix.Driver.Performance;

namespace RmEmulator.Devices
{
    public class EmulatedPerformanceMonitor : IPerformanceMonitor
    {
        public int NumberOfProcessors => 1;
        public int NumberOfCores => 1;
        public long TotalMemory => 510180000;
        public long TotalSwap => 0;

        public float GetProcessorTime() => 0.5f;

        public float GetProcessorTime(int processor) => 0.5f;

        public float GetProcessorTime(int processor, int core) => 0.5f;

        public long GetFreeMemory() => 459162000;

        public long GetFreeSwap() => 0;

        public string[] GetNetworkAdapters() => new[] { "Virtual adapter" };

        public long GetNetworkTxSpeed(string adapter) => 0;

        public long GetNetworkRxSpeed(string adapter) => 0;

        public long GetNetworkTxTotal(string adapter) => 0;

        public long GetNetworkRxTotal(string adapter) => 0;
    }
}