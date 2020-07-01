namespace ReMarkable.NET.Unix.Driver.Battery
{
    public interface IPowerSupplyMonitor
    {
        /// <summary>
        /// Gets the power supply percentage remaining
        /// </summary>
        /// <returns>A float between 0 and 1, inclusive</returns>
        float GetPercentage();

        /// <summary>
        /// Gets the charge, in Wh, of the power supply the last time it was full
        /// </summary>
        /// <returns>The charge, in Wh</returns>
        float GetChargeFull();

        /// <summary>
        /// Gets the charge, in Wh, of the power supply when full as designed
        /// </summary>
        /// <returns>The charge, in Wh</returns>
        float GetChargeFullDesign();

        /// <summary>
        /// Gets the instantaneous charge, in Wh, of the power supply
        /// </summary>
        /// <returns>The charge, in Wh</returns>
        float GetChargeNow();

        /// <summary>
        /// Gets the instantaneous voltage, in V, of the power supply
        /// </summary>
        /// <returns>The voltage, in V</returns>
        float GetVoltageNow();

        /// <summary>
        /// Gets the instantaneous current, in A, of the power supply
        /// </summary>
        /// <returns>The current, in A</returns>
        float GetCurrentNow();

        /// <summary>
        /// Gets the instantaneous temperature, in degrees C, of the power supply
        /// </summary>
        /// <returns>The temperature, in degrees C</returns>
        float GetTemperature();

        /// <summary>
        /// True if the power is flowing from an external source
        /// </summary>
        /// <returns></returns>
        bool IsOnline();

        /// <summary>
        /// True if the power supply is present in the device
        /// </summary>
        /// <returns></returns>
        bool IsPresent();

        /// <summary>
        /// Gets the status of the power supply
        /// </summary>
        /// <returns></returns>
        PowerSupplyStatus GetStatus();
    }
}
