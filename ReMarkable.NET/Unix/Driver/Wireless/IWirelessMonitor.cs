namespace ReMarkable.NET.Unix.Driver.Wireless
{
    /// <summary>
    ///     Provides an interface through which to monitor the state of the wireless network
    /// </summary>
    public interface IWirelessMonitor
    {
        /// <summary>
        ///     Gets the quality of the signal, e.g. the likelihood of a packet drop, SNR, % retries, etc.
        /// </summary>
        /// <returns>The quality between 0 and 1, inclusive</returns>
        float GetLinkQuality();

        /// <summary>
        ///     Gets the noise floor value in dBm when no packets are being transmitted
        /// </summary>
        /// <returns>The noise floor in dBm</returns>
        int GetSignalNoise();

        /// <summary>
        ///     Gets the strength of the signal in dBm
        /// </summary>
        /// <returns>The strength in dBm</returns>
        int GetSignalStrength();
    }
}