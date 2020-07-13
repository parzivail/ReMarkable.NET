using System.Runtime.InteropServices;

namespace ReMarkable.NET.Unix.Driver.Display.EinkController
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