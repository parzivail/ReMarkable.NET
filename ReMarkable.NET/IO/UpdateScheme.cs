namespace ReMarkable.NET.IO
{
    public enum UpdateScheme : uint
    {
        Snapshot = 0,
        Queue = 1,
        QueueAndMerge = 2
    }
}