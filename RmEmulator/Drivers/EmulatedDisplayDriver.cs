using ReMarkable.NET.Unix.Driver.Display;
using ReMarkable.NET.Unix.Driver.Display.EinkController;
using ReMarkable.NET.Unix.Driver.Display.Framebuffer;
using RmEmulator.Framebuffer;
using SixLabors.ImageSharp;

namespace RmEmulator.Drivers
{
    public class EmulatedDisplayDriver : IDisplayDriver
    {
        public int VirtualWidth => VisibleWidth;
        public int VirtualHeight => VisibleHeight;
        public int VisibleWidth { get; }
        public int VisibleHeight { get; }
        public IFramebuffer Framebuffer { get; }

        public EmulatedDisplayDriver(EmulatorWindow emulatorWindow, int visibleWidth, int visibleHeight)
        {
            VisibleWidth = visibleWidth;
            VisibleHeight = visibleHeight;

            Framebuffer = new EmulatedFramebuffer(visibleWidth, visibleHeight);
        }

        public void Refresh(Rectangle rectangle, WaveformMode mode)
        {
        }
    }
}