using System.Runtime.InteropServices;

namespace ReMarkable.NET.IO
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