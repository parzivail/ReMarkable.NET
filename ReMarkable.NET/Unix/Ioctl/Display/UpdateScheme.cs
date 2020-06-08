namespace ReMarkable.NET.Unix.Ioctl.Display
{
    public enum UpdateScheme : uint
    {
        Snapshot = 0,
        Queue = 1,
        QueueAndMerge = 2
    }
}