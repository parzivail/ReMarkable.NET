using ReMarkable.NET.Unix.Driver.Display;
using ReMarkable.NET.Unix.Driver.Display.EinkController;
using ReMarkable.NET.Unix.Driver.Display.Framebuffer;
using RmEmulator.Framebuffer;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace RmEmulator.Devices
{
    public class EmulatedDisplayDriver : IDisplayDriver
    {
        private readonly EmulatorWindow _emulatorWindow;

        public int VirtualWidth => VisibleWidth;
        public int VirtualHeight => VisibleHeight;
        public int VisibleWidth { get; }
        public int VisibleHeight { get; }
        public IFramebuffer Framebuffer { get; }

        public EmulatedDisplayDriver(EmulatorWindow emulatorWindow, int visibleWidth, int visibleHeight)
        {
            _emulatorWindow = emulatorWindow;
            VisibleWidth = visibleWidth;
            VisibleHeight = visibleHeight;

            Framebuffer = new EmulatedFramebuffer(_emulatorWindow, visibleWidth, visibleHeight);
        }

        public void Refresh(Rectangle rectangle, WaveformMode mode)
        {
            _emulatorWindow.RefreshRegion(rectangle, mode);
            EmulatedFramebuffer.FrontBuffer.Mutate(g => g.DrawImage(EmulatedFramebuffer.BackBuffer.Clone(g2 => g2.Crop(rectangle)), rectangle.Location, 1));
        }

        public void Draw(Image<Rgb24> image, Rectangle srcArea, Point destPoint, Rectangle refreshArea = default,
            WaveformMode mode = WaveformMode.Auto)
        {
            Framebuffer.Write(image, srcArea, destPoint);

            if (refreshArea == default)
            {
                refreshArea.Location = destPoint;
                refreshArea.Size = srcArea.Size;
            }

            Refresh(refreshArea, mode);
        }
    }
}