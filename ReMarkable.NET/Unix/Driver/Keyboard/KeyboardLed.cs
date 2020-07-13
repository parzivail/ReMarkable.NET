namespace ReMarkable.NET.Unix.Driver.Keyboard
{
    /// <summary>
    ///     Defines the possible event codes an attached keyboard can consume through the LED event
    /// </summary>
    public enum KeyboardLed
    {
        /// <summary>
        ///     The number lock LED
        /// </summary>
        NumLock = 0,

        /// <summary>
        ///     The caps lock LED
        /// </summary>
        CapsLock = 1,

        /// <summary>
        ///     The scroll lock LEDs
        /// </summary>
        ScrollLock = 2
    }
}