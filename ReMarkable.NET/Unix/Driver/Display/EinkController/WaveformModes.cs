using System.Runtime.InteropServices;

namespace ReMarkable.NET.Unix.Driver.Display.EinkController
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WaveformModes
    {
        public int ModeInit;
        public int ModeDu;
        public int ModeGc4;
        public int ModeGc8;
        public int ModeGc16;
        public int ModeGc32;
    }
}