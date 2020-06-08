using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ReMarkable.NET.IO
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
