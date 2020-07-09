using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ReMarkable.NET.Unix.Driver.Wireless
{
    public interface IWirelessMonitor
    {
        /// <summary>
        /// Gets the quality of the signal, e.g. the likelihood of a packet drop, SNR, % retries, etc.
        /// </summary>
        /// <returns>The quality between 0 and 1, inclusive</returns>
        float GetLinkQuality();

        /// <summary>
        /// Gets the strength of the signal in dBm
        /// </summary>
        /// <returns>The strength in dBm</returns>
        int GetSignalStrength();

        /// <summary>
        /// Gets the noise floor value in dBm when no packets are being transmitted
        /// </summary>
        /// <returns>The noise floor in dBm</returns>
        int GetSignalNoise();
    }

    public sealed class HardwareWirelessMonitor : IWirelessMonitor
    {
        /// <inheritdoc />
        public float GetLinkQuality()
        {
            var qual = GetQuality();
            return qual.QualLink / 70f;
        }

        /// <inheritdoc />
        public int GetSignalStrength() => GetQuality().QualLevel;

        /// <inheritdoc />
        public int GetSignalNoise() => GetQuality().QualNoise;

        private static WirelessQuality GetQuality()
        {
            var lines = File.ReadAllLines("/proc/net/wireless");

            var wlan0 = lines[2];
            var columns = Regex.Split(wlan0.Trim(), "\\s+");

            var qualLink = int.Parse(columns[2].Trim('.'));
            var qualLevel = int.Parse(columns[3].Trim('.'));
            var qualNoise = int.Parse(columns[4]);

            return new WirelessQuality(qualLink, qualLevel, qualNoise);
        }

        private class WirelessQuality
        {
            public int QualLink { get; }
            public int QualLevel { get; }
            public int QualNoise { get; }

            public WirelessQuality(in int qualLink, in int qualLevel, in int qualNoise)
            {
                QualLink = qualLink;
                QualLevel = qualLevel;
                QualNoise = qualNoise;
            }
        }
    }
}
