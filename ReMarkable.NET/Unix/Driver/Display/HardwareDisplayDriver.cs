using System;
using ReMarkable.NET.Unix.Driver.Display.EinkController;
using ReMarkable.NET.Unix.Driver.Display.Framebuffer;
using SixLabors.ImageSharp;

namespace ReMarkable.NET.Unix.Driver.Display
{
    public sealed class HardwareDisplayDriver : IDisposable, IDisplayDriver
    {
        private readonly SafeUnixHandle _handle;
        private readonly FbVarScreenInfo _vinfo;
        private readonly FbFixedScreenInfo _finfo;

        public int VisibleWidth { get; }
        public int VisibleHeight { get; }
        public int VirtualWidth { get; }
        public int VirtualHeight { get; }
        public IFramebuffer Framebuffer { get; }

        public string DevicePath { get; }

        internal HardwareDisplayDriver(string devicePath)
        {
            DevicePath = devicePath;
            
            _handle = UnsafeNativeMethods.Open(DevicePath, 0, UnixFileMode.WriteOnly);
            
            _vinfo = GetVarScreenInfo();
            _finfo = GetFixedScreenInfo();

            VisibleWidth = (int)_vinfo.VisibleResolutionX;
            VisibleHeight = (int)_vinfo.VisibleResolutionY;
            VirtualWidth = (int)_vinfo.VirtualResolutionX;
            VirtualHeight = (int)_vinfo.VirtualResolutionY;
            Framebuffer = new HardwareFramebuffer(devicePath, VisibleWidth, VisibleHeight, VirtualWidth, VirtualHeight);

            _vinfo.AccelFlags = 0x01;
            _vinfo.Width = 0xFFFFFFFF;
            _vinfo.Height  = 0xFFFFFFFF;
            _vinfo.Rotate = 1;
            _vinfo.PixClock = 160000000;
            _vinfo.VisibleResolutionX = 1872;
            _vinfo.VisibleResolutionY = 1404;
            _vinfo.LeftMargin = 32;
            _vinfo.RightMargin = 326;
            _vinfo.UpperMargin = 4;
            _vinfo.LowerMargin = 12;
            _vinfo.HSyncLen = 44;
            _vinfo.VSyncLen = 1;
            _vinfo.Sync = FbSync.None;
            _vinfo.VMode = FbVMode.NonInterlaced;
            _vinfo.BitsPerPixel = sizeof(short) * 8;
            _vinfo.AccelFlags = 0;

            PutVarScreenInfo(_vinfo);
        }

        public void Refresh(Rectangle rectangle, WaveformMode mode)
        {
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
                DisplayTemp = DisplayTemp.Papyrus,
                UpdateMode = UpdateMode.Full,
                UpdateMarker = 0,
                DitherMode = 0,
                QuantBit = 0,
                Flags = 0
            };

            if (DisplayIoctl.Ioctl(_handle, IoctlDisplayCommand.SendUpdate, ref data) == -1)
                throw new UnixException();
        }

        private FbVarScreenInfo GetVarScreenInfo()
        {
            var vinfo = new FbVarScreenInfo();
            if (DisplayIoctl.Ioctl(_handle, IoctlDisplayCommand.GetVariableScreenInfo, ref vinfo) == -1)
                throw new UnixException();
            return vinfo;
        }

        private void PutVarScreenInfo(FbVarScreenInfo vinfo)
        {
            if (DisplayIoctl.Ioctl(_handle, IoctlDisplayCommand.PutVariableScreenInfo, ref vinfo) == -1)
                throw new UnixException();
        }

        private FbFixedScreenInfo GetFixedScreenInfo()
        {
            var finfo = new FbFixedScreenInfo();
            if (DisplayIoctl.Ioctl(_handle, IoctlDisplayCommand.GetFixedScreenInfo, ref finfo) == -1)
                throw new UnixException();
            return finfo;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _handle?.Dispose();
            ((HardwareFramebuffer)Framebuffer)?.Dispose();
        }
    }
}
