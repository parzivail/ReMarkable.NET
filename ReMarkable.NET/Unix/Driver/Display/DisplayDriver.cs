using System;
using System.Runtime.InteropServices;
using ReMarkable.NET.Graphics;
using ReMarkable.NET.Unix.Driver.Display.EinkController;
using ReMarkable.NET.Unix.Driver.Display.Framebuffer;
using SixLabors.ImageSharp;

namespace ReMarkable.NET.Unix.Driver.Display
{
    public sealed class DisplayDriver : IDisposable
    {
        private readonly SafeUnixHandle _handle;
        private readonly FbVarScreenInfo _vinfo;
        private readonly FbFixedScreenInfo _finfo;

        public readonly int VisibleWidth;
        public readonly int VisibleHeight;
        public readonly Rgb565Framebuffer Framebuffer;

        public string DevicePath { get; }

        internal DisplayDriver(string devicePath)
        {
            DevicePath = devicePath;
            
            _handle = UnsafeNativeMethods.Open(DevicePath, 0, UnixFileMode.WriteOnly);
            
            _vinfo = GetVarScreenInfo();
            _finfo = GetFixedScreenInfo();

            VisibleWidth = (int)_vinfo.VisibleResolutionX;
            VisibleHeight = (int)_vinfo.VisibleResolutionY;
            Framebuffer = new Rgb565Framebuffer(devicePath, (int)_vinfo.VirtualResolutionX, (int)_vinfo.VirtualResolutionY);
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

            if (UnsafeNativeMethods.Ioctl(_handle, IoctlDisplayCommand.SendUpdate, ref data) == -1)
                throw new UnixException();
        }

        private FbVarScreenInfo GetVarScreenInfo()
        {
            var vinfo = new FbVarScreenInfo();
            if (UnsafeNativeMethods.Ioctl(_handle, IoctlDisplayCommand.GetVariableScreenInfo, ref vinfo) == -1)
                throw new UnixException();
            return vinfo;
        }

        private FbFixedScreenInfo GetFixedScreenInfo()
        {
            var finfo = new FbFixedScreenInfo();
            if (UnsafeNativeMethods.Ioctl(_handle, IoctlDisplayCommand.GetFixedScreenInfo, ref finfo) == -1)
                throw new UnixException();
            return finfo;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _handle?.Dispose();
            Framebuffer?.Dispose();
        }
    }
}
