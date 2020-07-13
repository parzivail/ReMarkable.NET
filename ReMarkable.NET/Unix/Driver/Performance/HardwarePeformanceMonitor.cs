using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ReMarkable.NET.Unix.Driver.Performance
{
    /// <summary>
    ///     Provides a set of methods to profile hardware performance metrics
    /// </summary>
    public class HardwarePeformanceMonitor : IPerformanceMonitor
    {
        /// <summary>
        ///     Contains the instantaneous CPU time-based measurements
        /// </summary>
        private readonly Dictionary<string, PerformanceMeasurement> _cpuMeasurements;

        /// <summary>
        ///     Contains the instantaneous network time-based measurements
        /// </summary>
        private readonly Dictionary<string, PerformanceMeasurement> _networkMeasurements;

        /// <inheritdoc />
        public int NumberOfCores { get; }

        /// <inheritdoc />
        public int NumberOfProcessors { get; }

        /// <inheritdoc />
        public long TotalMemory { get; }

        /// <inheritdoc />
        public long TotalSwap { get; }

        public HardwarePeformanceMonitor()
        {
            _cpuMeasurements = new Dictionary<string, PerformanceMeasurement>();
            var cpuMeasurements = GetCpuMeasurements();

            foreach (var (key, _) in cpuMeasurements)
            {
                _cpuMeasurements.Add($"idle-{key}", new PerformanceMeasurement());
                _cpuMeasurements.Add($"total-{key}", new PerformanceMeasurement());
            }

            NumberOfProcessors = 1;
            NumberOfCores = cpuMeasurements.Count - 1;

            var memoryMeasurements = GetMemoryMeasurements();

            TotalMemory = memoryMeasurements["MemTotal"];
            TotalSwap = memoryMeasurements["SwapTotal"];

            _networkMeasurements = new Dictionary<string, PerformanceMeasurement>();
            var networkMeasurements = GetNetworkMeasurements();

            foreach (var (key, _) in networkMeasurements)
            {
                _networkMeasurements.Add($"tx-{key}", new PerformanceMeasurement());
                _networkMeasurements.Add($"rx-{key}", new PerformanceMeasurement());
            }
        }

        /// <inheritdoc />
        public long GetFreeMemory()
        {
            var memoryMeasurements = GetMemoryMeasurements();
            return memoryMeasurements["MemAvailable"];
        }

        /// <inheritdoc />
        public long GetFreeSwap()
        {
            var memoryMeasurements = GetMemoryMeasurements();
            return memoryMeasurements["SwapFree"];
        }

        /// <inheritdoc />
        public string[] GetNetworkAdapters()
        {
            return GetNetworkMeasurements().Keys.ToArray();
        }

        /// <inheritdoc />
        public long GetNetworkRxSpeed(string adapter)
        {
            var nowBytes = GetNetworkMeasurements()[adapter].RxBytes;
            var measurement = _networkMeasurements[$"rx-{adapter}"].PushMeasurementPerSecond(nowBytes);

            return (long)measurement;
        }

        /// <inheritdoc />
        public long GetNetworkRxTotal(string adapter)
        {
            return GetNetworkMeasurements()[adapter].RxBytes;
        }

        /// <inheritdoc />
        public long GetNetworkTxSpeed(string adapter)
        {
            var nowBytes = GetNetworkMeasurements()[adapter].TxBytes;
            var measurement = _networkMeasurements[$"tx-{adapter}"].PushMeasurementPerSecond(nowBytes);

            return (long)measurement;
        }

        /// <inheritdoc />
        public long GetNetworkTxTotal(string adapter)
        {
            return GetNetworkMeasurements()[adapter].TxBytes;
        }

        /// <inheritdoc />
        public float GetProcessorTime()
        {
            var cpuMeasurements = GetCpuMeasurements();
            var cpuTotal = cpuMeasurements["cpu"];

            var idle = _cpuMeasurements["idle-cpu"].PushMeasurement(cpuTotal.Idle);
            var total = _cpuMeasurements["total-cpu"].PushMeasurement(cpuTotal.Total);

            return (float)(1 - idle / total);
        }

        /// <inheritdoc />
        public float GetProcessorTime(int processor)
        {
            if (processor != 0)
                throw new ArgumentException(nameof(processor));

            return GetProcessorTime();
        }

        /// <inheritdoc />
        public float GetProcessorTime(int processor, int core)
        {
            if (processor != 0)
                throw new ArgumentException(nameof(processor));

            var cpuMeasurements = GetCpuMeasurements();
            var cpuTotal = cpuMeasurements["cpu"];

            var idle = _cpuMeasurements[$"idle-cpu{core}"].PushMeasurement(cpuTotal.Idle);
            var total = _cpuMeasurements[$"total-cpu{core}"].PushMeasurement(cpuTotal.Total);

            return (float)(1 - idle / total);
        }

        private static Dictionary<string, CpuMeasurement> GetCpuMeasurements()
        {
            var d = new Dictionary<string, CpuMeasurement>();

            using (var sr = new StreamReader("/proc/stat"))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();

                    if (line == null || !line.StartsWith("cpu"))
                        break;

                    var columns = Regex.Split(line, "\\s+");

                    var cpuIdx = columns[0];

                    var totalTime = columns.Skip(1).Aggregate(0L, (i, s) => i + long.Parse(s));
                    var idleTime = long.Parse(columns[4]);

                    d.Add(cpuIdx, new CpuMeasurement(totalTime, idleTime));
                }
            }

            return d;
        }

        private static Dictionary<string, long> GetMemoryMeasurements()
        {
            var d = new Dictionary<string, long>();

            using (var sr = new StreamReader("/proc/meminfo"))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();

                    if (line == null)
                        continue;

                    var match = Regex.Match(line, "(.+):\\s+(\\d+) kB");

                    if (!match.Success)
                        continue;

                    // Kilobytes
                    d.Add(match.Groups[1].Value, long.Parse(match.Groups[2].Value) * 1000);
                }
            }

            return d;
        }

        private static Dictionary<string, NetDeviceInfo> GetNetworkMeasurements()
        {
            var d = new Dictionary<string, NetDeviceInfo>();

            using (var sr = new StreamReader("/proc/net/dev"))
            {
                // 2 header lines
                sr.ReadLine();
                sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();

                    if (line == null)
                        continue;

                    var columns = Regex.Split(line.Trim(), "\\s+");

                    var adapterName = columns[0];
                    adapterName = adapterName.Remove(adapterName.Length - 1);

                    var rxBytes = long.Parse(columns[1]);
                    var rxPackets = long.Parse(columns[2]);
                    var rxErrs = long.Parse(columns[3]);
                    var rxDrop = long.Parse(columns[4]);
                    var rxFifo = long.Parse(columns[5]);
                    var rxFrame = long.Parse(columns[6]);
                    var rxCompressed = long.Parse(columns[7]);
                    var rxMulticast = long.Parse(columns[8]);

                    var txBytes = long.Parse(columns[9]);
                    var txPackets = long.Parse(columns[10]);
                    var txErrs = long.Parse(columns[11]);
                    var txDrop = long.Parse(columns[12]);
                    var txFifo = long.Parse(columns[13]);
                    var txFrame = long.Parse(columns[14]);
                    var txCompressed = long.Parse(columns[15]);
                    var txMulticast = long.Parse(columns[16]);

                    d.Add(adapterName,
                        new NetDeviceInfo(rxBytes, rxPackets, rxErrs, rxDrop, rxFifo, rxFrame, rxCompressed,
                            rxMulticast, txBytes, txPackets, txErrs, txDrop, txFifo, txFrame, txCompressed,
                            txMulticast));
                }
            }

            return d;
        }
    }
}