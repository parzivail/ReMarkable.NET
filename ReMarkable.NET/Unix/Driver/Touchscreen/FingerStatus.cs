namespace ReMarkable.NET.Unix.Driver.Touchscreen
{
    /// <summary>
    ///     Defines the possible states in which the touchscreen can represent a finger
    /// </summary>
    public enum FingerStatus
    {
        /// <summary>
        ///     The finger is not in contact with the touchscreen
        /// </summary>
        Untracked,

        /// <summary>
        ///     The finger has just been removed from the touchscreen
        /// </summary>
        Up,

        /// <summary>
        ///     The finger has just come into contact with the touchscreen
        /// </summary>
        Down,

        /// <summary>
        ///     The finger is translating across the touchscreen
        /// </summary>
        Moving
    }
}