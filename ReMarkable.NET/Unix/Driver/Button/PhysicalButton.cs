namespace ReMarkable.NET.Unix.Driver.Button
{
    /// <summary>
    ///     Defines the individual buttons present on the device
    /// </summary>
    public enum PhysicalButton : ushort
    {
        /// <summary>
        ///     The home button
        /// </summary>
        Home = 102,

        /// <summary>
        ///     The left button
        /// </summary>
        Left = 105,

        /// <summary>
        ///     The right button
        /// </summary>
        Right = 106,

        /// <summary>
        ///     The power button
        /// </summary>
        Power = 116,

        /// <summary>
        ///     Not mapped to a physical button
        /// </summary>
        /// <remarks>
        ///     It's possible that this key code is used in place of an actual key code when a device is awoken from "light sleep"
        /// </remarks>
        WakeUp = 143
    }
}