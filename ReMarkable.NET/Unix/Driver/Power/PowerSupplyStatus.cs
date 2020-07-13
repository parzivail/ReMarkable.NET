namespace ReMarkable.NET.Unix.Driver.Power
{
    /// <summary>
    ///     Defines valid statuses for a power supply
    /// </summary>
    public enum PowerSupplyStatus
    {
        /// <summary>
        ///     It is impossible to determine the status of the power supply
        /// </summary>
        Unknown,

        /// <summary>
        ///     The power supply is attached to external power and is charging
        /// </summary>
        Charging,

        /// <summary>
        ///     The power supply is draining stored power
        /// </summary>
        Discharging,

        /// <summary>
        ///     The power supply is attached to external power and is neither charging nor full
        /// </summary>
        NotCharging,

        /// <summary>
        ///     The power supply contains the maximum possible power
        /// </summary>
        Full
    }
}