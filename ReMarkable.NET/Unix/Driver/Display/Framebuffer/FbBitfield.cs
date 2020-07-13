namespace ReMarkable.NET.Unix.Driver.Display.Framebuffer
{
    public struct FbBitfield
    {
        /// <summary>
        ///     beginning of bitfield
        /// </summary>
        public uint Offset;

        /// <summary>
        ///     length of bitfield
        /// </summary>
        public uint Length;

        /// <summary>
        ///     != 0 : Most significant bit is right
        /// </summary>
        public uint IsMsbRight;
    }
}