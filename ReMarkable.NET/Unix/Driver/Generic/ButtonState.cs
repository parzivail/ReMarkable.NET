namespace ReMarkable.NET.Unix.Driver.Generic
{
    /// <summary>
    ///     Defines the possible instantaneous states a button can have
    /// </summary>
    public enum ButtonState
    {
        /// <summary>
        ///     The button is not pressed
        /// </summary>
        Released = 0,

        /// <summary>
        ///     The button is pressed
        /// </summary>
        Pressed = 1,

        /// <summary>
        ///     If applicable, the button has been pressed long enough to produce repeating press events
        /// </summary>
        Repeat = 2
    }
}