namespace ReMarkable.NET.Unix.Driver.Touchscreen
{
    /// <summary>
    ///     Defines the possible event types the touchscreen can raise
    /// </summary>
    internal enum TouchscreenEventType
    {
        Syn = 0,
        Key = 1,
        Relative = 2,
        Absolute = 3
    }
}