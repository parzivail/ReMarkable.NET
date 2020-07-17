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

        public void Refresh(Rectangle rectangle, WaveformMode mode, DisplayTemp displayTemp, UpdateMode updateMode)
        {
            _emulatorWindow.RefreshRegion(rectangle, mode, displayTemp, updateMode);
        }

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
    }
}