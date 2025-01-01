using ReMarkable.NET.Unix.Driver.Performance;
using System;

namespace RmEmulator.Devices
{
    public class EmulatedPerformanceMonitor : IPerformanceMonitor
    {
        public int NumberOfProcessors => 1;
        public int NumberOfCores => 1;
        public long TotalMemory => 510180000;
        public long TotalSwap => 0;

        public float GetProcessorTime() => MockedCPU();

        public float GetProcessorTime(int processor) => MockedCPU();

        public float GetProcessorTime(int processor, int core) => MockedCPU();

        public long GetFreeMemory() => MockedRAM();

        public long GetFreeSwap() => 0;

        public string[] GetNetworkAdapters() => new[] { "Virtual adapter" };

        public long GetNetworkTxSpeed(string adapter) => 0;

        public long GetNetworkRxSpeed(string adapter) => 0;

        public long GetNetworkTxTotal(string adapter) => 0;

        public long GetNetworkRxTotal(string adapter) => 0;


        private float MockedCPU()
        {
            return (float)random.Next(0, 100) / 100;
        }

        private long MockedRAM()
        {
            return (long)(random.NextDouble() * TotalMemory);
        }

        private Random random = new Random();

        public EmulatedPerformanceMonitor()
        {
            random = new Random(DateTime.Now.Millisecond);
        }
    }
}