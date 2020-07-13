namespace ReMarkable.NET.Unix.Driver.Touchscreen
{
    /// <summary>
    ///     Defines the possible event codes the touchscreen can raise through the ABS event
    /// </summary>
    public enum TouchscreenEventAbsCode
    {
        /// <summary>
        ///     Reports the distance of a finger from the touchscreen
        /// </summary>
        Distance = 25,

        /// <summary>
        ///     Reports the slot ID of a finger
        /// </summary>
        MultiTouchSlot = 47,

        /// <summary>
        ///     Reports the major axis of a multi-touch pair
        /// </summary>
        MultiTouchTouchMajor = 48,

        /// <summary>
        ///     Reports the minor axis of a multi-touch pair
        /// </summary>
        MultiTouchTouchMinor = 49,

        /// <summary>
        ///     Reports the orientation of a multi-touch pair
        /// </summary>
        MultiTouchOrientation = 52,

        /// <summary>
        ///     Reports the X position of a finger
        /// </summary>
        MultiTouchPositionX = 53,

        /// <summary>
        ///     Reports the Y position of a finger
        /// </summary>
        MultiTouchPositionY = 54,

        /// <summary>
        ///     Reports the tool ID of a finger
        /// </summary>
        MultiTouchToolType = 55,

        /// <summary>
        ///     Reports the tracking ID of the finger event
        /// </summary>
        MultiTouchTrackingId = 57,

        /// <summary>
        ///     Reports the pressure of the finger
        /// </summary>
        MultiTouchPressure = 58
    }
}