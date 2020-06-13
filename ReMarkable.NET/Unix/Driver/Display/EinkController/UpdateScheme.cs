namespace ReMarkable.NET.Unix.Driver.Display.EinkController
{
    public enum UpdateScheme : uint
    {
        Snapshot = 0,
        Queue = 1,
        QueueAndMerge = 2
    }
}