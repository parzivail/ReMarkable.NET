namespace ReMarkable.NET.Unix.Driver.Keyboard
{
    /// <summary>
    ///     Defines the possible event types an attached keyboard can raise
    /// </summary>
    internal enum KeyboardEventType
    {
        Syn = 0,
        Key = 1,
        Msc = 2,
        Led = 17
    }
}