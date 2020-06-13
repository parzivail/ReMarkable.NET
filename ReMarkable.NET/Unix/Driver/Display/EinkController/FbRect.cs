using System.Runtime.InteropServices;

namespace ReMarkable.NET.Unix.Driver.Display.EinkController
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FbRect
    {
        public uint Y;
        public uint X;
        public uint Width;
        public uint Height;
    }
}