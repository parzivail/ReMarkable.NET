using SixLabors.ImageSharp.PixelFormats;

namespace ReMarkable.NET.Graphics
{
    /// <summary>
    ///     Provides utilities for packing and unpacking <see cref="ushort" /> encoded RGB565 colors
    /// </summary>
    public static class Rgb565
    {
        /// <summary>
        ///     The blue component bitmask
        /// </summary>
        private const ushort MaskB = MaxB;

        /// <summary>
        ///     The green component bitmask
        /// </summary>
        private const ushort MaskG = MaxG << 6;

        /// <summary>
        ///     The red component bitmask
        /// </summary>
        private const ushort MaskR = MaxR << 11;

        /// <summary>
        ///     The blue component bits
        /// </summary>
        private const ushort MaxB = 0b11111;

        /// <summary>
        ///     The green component bits
        /// </summary>
        private const ushort MaxG = 0b111111;

        /// <summary>
        ///     The red component bits
        /// </summary>
        private const ushort MaxR = 0b11111;

        /// <summary>
        ///     Packs individual components of a 24-bit RGB values into a 16-bit RGB565 <see cref="ushort" />
        /// </summary>
        /// <param name="r">The red component byte</param>
        /// <param name="g">The green component byte</param>
        /// <param name="b">The blue component byte</param>
        /// <returns>A RGB565-encoded <see cref="ushort" /></returns>
        public static ushort Pack(byte r, byte g, byte b)
        {
            return (ushort)(((r << 8) & MaskR) | ((g << 3) & MaskG) | ((b >> 3) & MaskB));
        }

        /// <summary>
        ///     Unpacks a 16-bit RGB565 value into a 24-bit RGB value
        /// </summary>
        /// <param name="components">The RGB565 encoded value</param>
        /// <returns>A 24-bit <see cref="Rgb24" /></returns>
        public static Rgb24 Unpack(ushort components)
        {
            return new Rgb24((byte)(((components >> 11) & MaxR) << 3), (byte)(((components >> 5) & MaxG) << 2),
                (byte)((components & MaxB) << 3));
        }
    }
}