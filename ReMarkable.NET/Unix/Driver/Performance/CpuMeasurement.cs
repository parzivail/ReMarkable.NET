namespace ReMarkable.NET.Unix.Driver.Performance
{
    /// <summary>
    ///     Contains data associated with an instantaneous CPU measurement
    /// </summary>
    internal class CpuMeasurement
    {
        /// <summary>
        ///     The idle processor time for this sample
        /// </summary>
        public long Idle { get; }

        /// <summary>
        ///     The total processor time for this sample
        /// </summary>
        public long Total { get; }

        /// <summary>
        ///     Creates a new <see cref="CpuMeasurement" />
        /// </summary>
        /// <param name="total">The total processor time for this sample</param>
        /// <param name="idle">The idle processor time for this sample</param>
        public CpuMeasurement(long total, long idle)
        {
            Total = total;
            Idle = idle;
        }
    }
}