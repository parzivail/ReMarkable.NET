namespace ReMarkable.NET.Unix.Driver.Digitizer
{
    /// <summary>
    ///     Defines the possible event codes the digitizer can raise through the ABS event
    /// </summary>
    public enum DigitizerEventAbsCode
    {
        /// <summary>
        ///     Reports the X position of the stylus
        /// </summary>
        AbsX = 0,

        /// <summary>
        ///     Reports the Y position of the stylus
        /// </summary>
        AbsY = 1,

        /// <summary>
        ///     Reports the pressure of the stylus
        /// </summary>
        AbsPressure = 24,

        /// <summary>
        ///     Reports the distance from the stylus to the digitizer
        /// </summary>
        AbsDistance = 25,

        /// <summary>
        ///     Reports the tilt of the stylus in the X direction
        /// </summary>
        AbsTiltX = 26,

        /// <summary>
        ///     Reports the tilt of the stylus in the Y direction
        /// </summary>
        AbsTiltY = 27
    }
}