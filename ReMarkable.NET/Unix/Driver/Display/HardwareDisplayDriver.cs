using System;
using ReMarkable.NET.Unix.Driver.Display.EinkController;
using ReMarkable.NET.Unix.Driver.Display.Framebuffer;
using ReMarkable.NET.Unix.Stream;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ReMarkable.NET.Unix.Driver.Display
{
    /// <summary>
    ///     Provides methods for interacting with the hardware display installed in the device
    /// </summary>
    public sealed class HardwareDisplayDriver : IDisposable, IDisplayDriver
    {
        /// <summary>
        ///     The device handle through which IOCTL commands can be issued
        /// </summary>
        private readonly SafeUnixHandle _handle;

        /// <summary>
        ///     The update marker ID returned by the device
        /// </summary>
        private uint _updateMarker = 0;

        /// <summary>
        ///     The device handle location
        /// </summary>
        public string DevicePath { get; }

        /// <inheritdoc />
        public IFramebuffer Framebuffer { get; }

        /// <inheritdoc />
        public int VirtualHeight { get; }

        /// <inheritdoc />
        public int VirtualWidth { get; }

        /// <inheritdoc />
        public int VisibleHeight { get; }

        /// <inheritdoc />
        public int VisibleWidth { get; }

        /// <summary>
        ///     Creates a new <see cref="HardwareDisplayDriver" />
        /// </summary>
        /// <param name="devicePath">The device handle location</param>
        public HardwareDisplayDriver(string devicePath)
        {
            DevicePath = devicePath;

            _handle = NativeUnixStreamMethods.Open(DevicePath, 0, UnixFileMode.WriteOnly);

            var vinfo = GetVarScreenInfo();

            VisibleWidth = (int)vinfo.VisibleResolutionX;
            VisibleHeight = (int)vinfo.VisibleResolutionY;
            VirtualWidth = (int)vinfo.VirtualResolutionX;
            VirtualHeight = (int)vinfo.VirtualResolutionY;
            Framebuffer = new HardwareFramebuffer(devicePath, VisibleWidth, VisibleHeight, VirtualWidth, VirtualHeight);

            vinfo.AccelFlags = 0x01;
            vinfo.Width = 0xFFFFFFFF;
            vinfo.Height = 0xFFFFFFFF;
            vinfo.Rotate = 1;
            vinfo.PixClock = 160000000;
            vinfo.VisibleResolutionX = 1872;
            vinfo.VisibleResolutionY = 1404;
            vinfo.LeftMargin = 32;
            vinfo.RightMargin = 326;
            vinfo.UpperMargin = 4;
            vinfo.LowerMargin = 12;
            vinfo.HSyncLen = 44;
            vinfo.VSyncLen = 1;
            vinfo.Sync = FbSync.None;
            vinfo.VMode = FbVMode.NonInterlaced;
            vinfo.BitsPerPixel = sizeof(short) * 8;

            PutVarScreenInfo(vinfo);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _handle?.Dispose();
            ((HardwareFramebuffer)Framebuffer)?.Dispose();
        }

        /// <inheritdoc />
        public void Draw(Image<Rgb24> image, Rectangle srcArea, Point destPoint, Rectangle refreshArea = default,
            WaveformMode waveformMode = WaveformMode.Auto, DisplayTemp displayTemp = DisplayTemp.Papyrus, UpdateMode updateMode = UpdateMode.Partial)
        {
            Framebuffer.Write(image, srcArea, destPoint);

            if (refreshArea == default)
            {
                refreshArea.Location = destPoint;
                refreshArea.Size = srcArea.Size;
            }

            Refresh(refreshArea, waveformMode, displayTemp, updateMode);
        }

        /// <summary>
        ///     Reads fixed device information from the display
        /// </summary>
        /// <returns>A populated <see cref="FbFixedScreenInfo" /></returns>
        public FbFixedScreenInfo GetFixedScreenInfo()
        {
            var finfo = new FbFixedScreenInfo();
            if (DisplayIoctl.Ioctl(_handle, IoctlDisplayCommand.GetFixedScreenInfo, ref finfo) == -1)
                throw new UnixException();
            return finfo;
        }

        /// <summary>
        ///     Reads variable device information from the display
        /// </summary>
        /// <returns>A populated <see cref="FbVarScreenInfo" /></returns>
        public FbVarScreenInfo GetVarScreenInfo()
        {
            var vinfo = new FbVarScreenInfo();
            if (DisplayIoctl.Ioctl(_handle, IoctlDisplayCommand.GetVariableScreenInfo, ref vinfo) == -1)
                throw new UnixException();
            return vinfo;
        }

        /// <summary>
        ///     Writes variable device to the display
        /// </summary>
        /// <param name="vinfo">A populated <see cref="FbVarScreenInfo" /></param>
        public void PutVarScreenInfo(FbVarScreenInfo vinfo)
        {
            if (DisplayIoctl.Ioctl(_handle, IoctlDisplayCommand.PutVariableScreenInfo, ref vinfo) == -1)
                throw new UnixException();
        }

        /// <inheritdoc />
        public void Refresh(Rectangle rectangle, WaveformMode mode, DisplayTemp displayTemp, UpdateMode updateMode)
        {
            Framebuffer.ConstrainRectangle(ref rectangle);
            var data = new FbUpdateData
            {
                UpdateRegion = new FbRect
                {
                    X = (uint)rectangle.X,
                    Y = (uint)rectangle.Y,
                    Width = (uint)rectangle.Width,
                    Height = (uint)rectangle.Height
                },
                WaveformMode = mode,
                DisplayTemp = displayTemp,
                UpdateMode = updateMode,
                UpdateMarker = _updateMarker,
                DitherMode = 0,
                QuantBit = 0,
                Flags = 0
            };

            var retCode = DisplayIoctl.Ioctl(_handle, IoctlDisplayCommand.SendUpdate, ref data);
            if (retCode == -1)
                throw new UnixException();

            _updateMarker = (uint) retCode;
        }
    }
}