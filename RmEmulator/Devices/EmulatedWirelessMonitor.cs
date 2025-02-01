using System;
using ReMarkable.NET.Unix.Driver.Wireless;

namespace RmEmulator.Devices
{
    public class EmulatedWirelessMonitor : IWirelessMonitor
    {
        /// <inheritdoc />
        public float GetLinkQuality()
        {
            return (float)random.Next(0, 100)/100;
        }

        /// <inheritdoc />
        public int GetSignalStrength() {
            return random.Next(-70, 0);
        }

        /// <inheritdoc />
        public int GetSignalNoise()
        {
            return random.Next(-256, 0);
        }

        private readonly Random random;
        public EmulatedWirelessMonitor()
        {
            random = new Random(DateTime.Now.Millisecond);
        }
    }
}
