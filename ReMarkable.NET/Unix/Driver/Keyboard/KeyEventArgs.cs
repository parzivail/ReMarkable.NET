namespace ReMarkable.NET.Unix.Driver.Keyboard
{
    /// <summary>
    ///     Contains data related to generic key events raised by keyboards
    /// </summary>
    public class KeyEventArgs
    {
        /// <summary>
        ///     The key that raised the event
        /// </summary>
        public KeyboardKey Key { get; }

        /// <summary>
        ///     Creates a new <see cref="KeyEventArgs" />
        /// </summary>
        /// <param name="key">The key that raised the event</param>
        public KeyEventArgs(KeyboardKey key)
        {
            Key = key;
        }
    }
}