using System.IO;
using System.Text.RegularExpressions;

namespace ReMarkable.NET.Unix.Driver.Wireless
{
    /// <summary>
    ///     Provides a set of methods to monitor the installed wireless networking hardware
    /// </summary>
    public sealed class HardwareWirelessMonitor : IWirelessMonitor
    {
        /// <inheritdoc />
        public float GetLinkQuality()
        {
            var qual = GetQuality();
            return qual.QualLink / 70f;
        }

        /// <inheritdoc />
        public int GetSignalNoise()
        {
            return GetQuality().QualNoise;
        }

        /// <inheritdoc />
        public int GetSignalStrength()
        {
            return GetQuality().QualLevel;
        }

        /// <summary>
        ///     Parses the wireless network status file
        /// </summary>
        /// <returns>A populated <see cref="WirelessQuality" /> object</returns>
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

        /// <summary>
        ///     Contains data related to wireless network quality
        /// </summary>
        private class WirelessQuality
        {
            /// <summary>
            ///     The signal gain
            /// </summary>
            public int QualLevel { get; }

            /// <summary>
            ///     The link quality
            /// </summary>
            public int QualLink { get; }

            /// <summary>
            ///     The signal noise baseline
            /// </summary>
            public int QualNoise { get; }

            /// <summary>
            ///     Creates a new <see cref="WirelessQuality" />
            /// </summary>
            /// <param name="qualLink">The link quality</param>
            /// <param name="qualLevel">The signal gain</param>
            /// <param name="qualNoise">The signal noise baseline</param>
            public WirelessQuality(in int qualLink, in int qualLevel, in int qualNoise)
            {
                QualLink = qualLink;
                QualLevel = qualLevel;
                QualNoise = qualNoise;
            }
        }
    }
}