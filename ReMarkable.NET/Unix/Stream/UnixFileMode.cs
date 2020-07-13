namespace ReMarkable.NET.Unix.Stream
{
    /// <summary>
    ///     Defines the possible flags a Unix file stream can be opened with
    /// </summary>
    public enum UnixFileMode
    {
        ReadOnly = 0,
        WriteOnly = 1,
        ReadWrite = 2
    }
}