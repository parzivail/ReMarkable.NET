namespace ReMarkable.NET.Unix.Driver.Display.Framebuffer
{
    public struct FbVarScreenInfo
    {
        public uint VisibleResolutionX;

        public uint VisibleResolutionY;

        public uint VirtualResolutionX;

        public uint VirtualResolutionY;

        public uint VisibleOffsetX;

        public uint VisibleOffsetY;

        public uint BitsPerPixel;

        /// <summary>
        ///     0 = color, 1 = grayscale,
        /// </summary>
        public uint IsGrayscale;

        public FbBitfield Red;

        public FbBitfield Green;

        public FbBitfield Blue;

        public FbBitfield Transparency;

        /// <summary>
        ///     != 0 Non standard pixel format
        /// </summary>
        public uint NonStandardPixelFormat;

        /// <summary>
        ///     see FB_ACTIVATE_*
        /// </summary>
        public uint Activate;

        /// <summary>
        ///     height of picture in mm
        /// </summary>
        public uint Height;

        /// <summary>
        ///     width of picture in mm
        /// </summary>
        public uint Width;

        /// <summary>
        ///     (OBSOLETE) see fb_info.flags
        /// </summary>
        public uint AccelFlags;

        /// <summary>
        ///     pixel clock in ps (picoseconds)
        /// </summary>
        public uint PixClock;

        /// <summary>
        ///     time from sync to picture in pixclocks
        /// </summary>
        public uint LeftMargin;

        /// <summary>
        ///     time from picture to sync in pixclocks
        /// </summary>
        public uint RightMargin;

        /// <summary>
        ///     time from sync to picture in pixclocks
        /// </summary>
        public uint UpperMargin;

        /// <summary>
        ///     time from picture to sync in pixclocks
        /// </summary>
        public uint LowerMargin;

        /// <summary>
        ///     length of horizontal sync in pixclocks
        /// </summary>
        public uint HSyncLen;

        /// <summary>
        ///     length of vertical sync in pixclocks
        /// </summary>
        public uint VSyncLen;

        /// <summary>
        ///     see FB_SYNC_*
        /// </summary>
        public FbSync Sync;

        /// <summary>
        ///     see FB_VMODE_*
        /// </summary>
        public FbVMode VMode;

        /// <summary>
        ///     angle we rotate counter clockwise
        /// </summary>
        public uint Rotate;

        /// <summary>
        ///     colorspace for FOURCC-based modes
        /// </summary>
        public uint Colorspace;

        /// <summary>
        ///     Reserved for future compatibility
        /// </summary>
        public uint Reserved0;

        /// <summary>
        ///     Reserved for future compatibility
        /// </summary>
        public uint Reserved1;

        /// <summary>
        ///     Reserved for future compatibility
        /// </summary>
        public uint Reserved2;

        /// <summary>
        ///     Reserved for future compatibility
        /// </summary>
        public uint Reserved3;
    }
}