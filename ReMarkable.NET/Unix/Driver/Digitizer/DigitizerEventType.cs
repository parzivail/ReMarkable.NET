namespace ReMarkable.NET.Unix.Driver.Digitizer
{
    /// <summary>
    ///     Defines the possible event types the digitizer can raise
    /// </summary>
    internal enum DigitizerEventType
    {
        Syn = 0,
        Key = 1,
        Abs = 3
    }
}