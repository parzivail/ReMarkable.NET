using System;
using System.Collections.Generic;
using System.Text;
using ReMarkable.NET.Unix.Driver.Wireless;

namespace RmEmulator.Devices
{
    public class EmulatedWirelessMonitor : IWirelessMonitor
    {
        /// <inheritdoc />
        public float GetLinkQuality() => 1;

        /// <inheritdoc />
        public int GetSignalStrength() => -45;

        /// <inheritdoc />
        public int GetSignalNoise() => -256;
    }
}
