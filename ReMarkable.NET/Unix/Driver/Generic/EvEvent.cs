using System.Runtime.InteropServices;

namespace ReMarkable.NET.Unix.Driver.Generic
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct EvEvent
    {
        [FieldOffset(0)]
        public uint TimeWholeSeconds;

        [FieldOffset(4)]
        public uint TimeFractionMicroseconds;

        [FieldOffset(8)]
        public ushort Type;

        [FieldOffset(10)]
        public ushort Code;

        [FieldOffset(12)]
        public int Value;
    }
}