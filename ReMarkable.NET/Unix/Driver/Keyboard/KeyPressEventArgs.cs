namespace ReMarkable.NET.Unix.Driver.Keyboard
{
    /// <summary>
    ///     Contains data related to repeatable key events raised by keyboards
    /// </summary>
    public class KeyPressEventArgs : KeyEventArgs
    {
        /// <summary>
        ///     Whether or not the raised event was a software repeat or a hardware state change
        /// </summary>
        public bool Repeat { get; }

        /// <summary>
        ///     Creates a new <see cref="KeyPressEventArgs" />
        /// </summary>
        /// <param name="key">The key that raised the event</param>
        /// <param name="repeat">Whether or not the raised event was a repeat event</param>
        public KeyPressEventArgs(KeyboardKey key, bool repeat) : base(key)
        {
            Repeat = repeat;
        }
    }
}