using System.Runtime.InteropServices;

namespace ReMarkable.NET.Unix.Driver
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EvEvent
    {
        public uint TimeWholeSeconds;
        public uint TimeFractionMicroseconds;
        public ushort Type;
        public ushort Code;
        public int Value;
    }
}