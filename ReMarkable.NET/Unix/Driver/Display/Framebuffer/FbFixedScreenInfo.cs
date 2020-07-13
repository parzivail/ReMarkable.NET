namespace ReMarkable.NET.Unix.Driver.Display.Framebuffer
{
    public struct FbFixedScreenInfo
    {
        /// <summary>
        ///     identification string eg "TT Builtin"
        /// </summary>
        public unsafe fixed char Id[16];

        /// <summary>
        ///     Start of frame buffer mem (physical address)
        /// </summary>
        public ulong SmemStart;

        /// <summary>
        ///     Length of frame buffer mem
        /// </summary>
        public uint SmemLen;

        /// <summary>
        ///     see FB_TYPE_*
        /// </summary>
        public uint Type;

        /// <summary>
        ///     Interleave for interleaved Planes
        /// </summary>
        public uint TypeAux;

        /// <summary>
        ///     see FB_VISUAL_*
        /// </summary>
        public uint Visual;

        /// <summary>
        ///     zero if no hardware panning
        /// </summary>
        public ushort XPanStep;

        /// <summary>
        ///     zero if no hardware panning
        /// </summary>
        public ushort YPanStep;

        /// <summary>
        ///     zero if no hardware ywrap
        /// </summary>
        public ushort YWrapStep;

        /// <summary>
        ///     length of a line in bytes
        /// </summary>
        public uint LineLength;

        /// <summary>
        ///     Start of Memory Mapped I/O (physical address)
        /// </summary>
        public ulong MmioStart;

        /// <summary>
        ///     Length of Memory Mapped I/O
        /// </summary>
        public uint MmioLen;

        /// <summary>
        ///     Indicate to driver which specific chip/card we have
        /// </summary>
        public uint Accel;

        /// <summary>
        ///     see FB_CAP_*
        /// </summary>
        public ushort Capabilities;

        /// <summary>
        ///     Reserved for future compatibility
        /// </summary>
        public ushort Reserved0;

        /// <summary>
        ///     Reserved for future compatibility
        /// </summary>
        public ushort Reserved1;
    }
}