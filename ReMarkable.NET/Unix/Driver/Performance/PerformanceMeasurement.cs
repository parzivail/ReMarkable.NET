using System;

namespace ReMarkable.NET.Unix.Driver.Performance
{
    /// <summary>
    ///     Provides methods for calculating static and time-based performance metrics
    /// </summary>
    internal class PerformanceMeasurement
    {
        private DateTime _previousTime;
        private double _previousValue;

        /// <summary>
        ///     Calculates a differential based on the previous and new values
        /// </summary>
        /// <param name="measurement">The new value to compare to the old value</param>
        /// <returns>The calculated delta</returns>
        public double PushMeasurement(double measurement)
        {
            var dM = measurement - _previousValue;
            _previousValue = measurement;

            return dM;
        }

        /// <summary>
        ///     Calculates a time-based differential based on the previous and new values and the previous and current time
        /// </summary>
        /// <param name="measurement">The new value to compare to the old value</param>
        /// <returns>The calculated delta in units per second</returns>
        public double PushMeasurementPerSecond(double measurement)
        {
            var time = DateTime.Now;

            var dT = time - _previousTime;
            var dM = measurement - _previousValue;

            _previousTime = time;
            _previousValue = measurement;

            return dM / dT.TotalSeconds;
        }
    }
}