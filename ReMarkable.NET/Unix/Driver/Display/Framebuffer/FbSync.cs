using System;

namespace ReMarkable.NET.Unix.Driver.Display.Framebuffer
{
    [Flags]
    public enum FbSync : uint
    {
        None = 0,
        HorizontalHighActive = 1,
        VerticalHighActive = 2,
        External = 4,
        CompositeHighActive = 8,
        BroadcastTimings = 16
    }
}