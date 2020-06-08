using System.Runtime.InteropServices;

namespace ReMarkable.NET.Unix.Ioctl.Display
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FbUpdateData
    {
        public FbRect UpdateRegion;
        public WaveformMode WaveformMode;
        public UpdateMode UpdateMode;
        public uint UpdateMarker;
        public DisplayTemp DisplayTemp;
        public uint Flags;
        public int DitherMode;
        public int QuantBit;
        public FbAltBufferData AltData;
    }
}
