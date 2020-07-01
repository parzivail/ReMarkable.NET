using System;

namespace ReMarkable.NET.Unix.Driver.Performance
{
    internal class Measurement
    {
        private DateTime _previousTime;
        private double _previousValue;

        public double PushMeasurementPerSecond(double measurement)
        {
            var time = DateTime.Now;

            var dT = time - _previousTime;
            var dM = measurement - _previousValue;

            _previousTime = time;
            _previousValue = measurement;

            return dM / dT.TotalSeconds;
        }

        public double PushMeasurement(double measurement)
        {
            var dM = measurement - _previousValue;
            _previousValue = measurement;

            return dM;
        }
    }
}