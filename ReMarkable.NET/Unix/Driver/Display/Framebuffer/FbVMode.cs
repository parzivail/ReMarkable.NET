using System;

namespace ReMarkable.NET.Unix.Driver.Display.Framebuffer
{
    [Flags]
    public enum FbVMode : uint
    {
        NonInterlaced = 0,
        Interlaced = 1,
        Double = 2,
        OddFieldFirst = 4,
        YWrap = 256,
        SmoothXPan = 512
    }
}