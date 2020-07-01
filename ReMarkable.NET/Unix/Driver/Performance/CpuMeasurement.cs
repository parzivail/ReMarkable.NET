namespace ReMarkable.NET.Unix.Driver.Performance
{
    internal class CpuMeasurement
    {
        public long Total { get; }
        public long Idle { get; }

        public CpuMeasurement(long total, long idle)
        {
            Total = total;
            Idle = idle;
        }
    }
}