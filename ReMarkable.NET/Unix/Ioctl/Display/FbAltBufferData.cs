using System.Runtime.InteropServices;

namespace ReMarkable.NET.Unix.Ioctl.Display
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FbAltBufferData
    {
        public uint PhysicalAddress;
        public uint Width;
        public uint Height;
        public FbRect AltUpdateRegion;
    }
}