namespace ReMarkable.NET.Graphics
{
    public static class Rgb565
    {
        private const ushort MaxR = 0b11111;
        private const ushort MaxG = 0b111111;
        private const ushort MaxB = 0b11111;

        private const ushort MaskR = MaxR << 11;
        private const ushort MaskG = MaxG << 6;
        private const ushort MaskB = MaxB;

        public static ushort Pack(byte r, byte g, byte b)
        {
            return (ushort)(((r << 8) & MaskR) | ((g << 3) & MaskG) | ((b >> 3) & MaskB));
        }

        public static (byte r, byte b, byte g) Unpack(ushort components)
        {
            return ((byte)(((components >> 11) & MaxR) << 3), (byte)(((components >> 5) & MaxG) << 2), (byte)((components & MaxB) << 3));
        }
    }
}