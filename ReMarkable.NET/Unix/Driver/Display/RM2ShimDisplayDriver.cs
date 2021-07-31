using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using ReMarkable.NET.Unix.Driver.Display.EinkController;
using ReMarkable.NET.Unix.Driver.Display.Framebuffer;
using ReMarkable.NET.Unix.Stream;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ReMarkable.NET.Unix.Driver.Display
{
    /// <summary>
    ///     Provides methods for interacting with the rm2fb client used on a rm2 device
    /// </summary>
    public sealed class RM2ShimDisplayDriver : IDisposable, IDisplayDriver
    {
        // Because of the way dotnet works we need to manually invoke functions normally hooked by rm2fb

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [DllImport("librm2fb_client.so.1.0.1", EntryPoint = "close", SetLastError = true)]
        private static extern int Close(IntPtr handle);

        [DllImport("librm2fb_client.so.1.0.1", EntryPoint = "open", SetLastError = false)]
        [SuppressMessage("Globalization", "CA2101:Specify marshaling for P/Invoke string arguments", Justification =
            "Specifying a marshaling breaks rM compatability")]
        private static extern SafeUnixHandle Open(string path, uint flags, UnixFileMode mode);

        [DllImport("librm2fb_client.so.1.0.1", EntryPoint = "ioctl", SetLastError = false)]
        public static extern int Ioctl(SafeHandle handle, IoctlDisplayCommand code, ref FbUpdateData data);

        /// <summary>
        ///     The device handle through which IOCTL commands can be issued
        /// </summary>
        private readonly SafeUnixHandle _handle;

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
        ///     Creates a new <see cref="RM2ShimDisplayDriver" />
        /// </summary>
        /// <param name="devicePath">The device handle location</param>
        public RM2ShimDisplayDriver()
        {
            var devicePath = "/dev/fb0";

            _handle = Open(devicePath, 0, UnixFileMode.WriteOnly);

            VisibleWidth = 1404;
            VisibleHeight = 1872;
            VirtualWidth = 1404;
            VirtualHeight = 1872;
            Framebuffer = new HardwareFramebuffer(devicePath, VisibleWidth, VisibleHeight, VirtualWidth, VirtualHeight);
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
                UpdateMarker = 0,
                DitherMode = 0,
                QuantBit = 0,
                Flags = 0
            };

            var retCode = Ioctl(_handle, IoctlDisplayCommand.SendUpdate, ref data);
            if (retCode == -1)
                throw new UnixException();
        }
    }
}