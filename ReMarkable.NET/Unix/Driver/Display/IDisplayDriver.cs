using ReMarkable.NET.Unix.Driver.Display.EinkController;
using ReMarkable.NET.Unix.Driver.Display.Framebuffer;
using SixLabors.ImageSharp;

namespace ReMarkable.NET.Unix.Driver.Display
{
    public interface IDisplayDriver
    {
        /// <summary>
        /// The horizontal resolution of the physically visible portion of the screen
        /// </summary>
        int VisibleWidth { get; }

        /// <summary>
        /// The vertical resolution of the physically visible portion of the screen
        /// </summary>
        int VisibleHeight { get; }

        /// <summary>
        /// The horizontal resolution of the backing framebuffer
        /// </summary>
        int VirtualWidth { get; }

        /// <summary>
        /// The vertical resolution of the backing framebuffer
        /// </summary>
        int VirtualHeight { get; }
        IFramebuffer Framebuffer { get; }

        /// <summary>
        /// Draws the contents of the internal buffer to the display device
        /// </summary>
        /// <param name="rectangle">The region of the buffer to be drawn to the device</param>
        /// <param name="mode">The update waveform used to refresh the region</param>
        void Refresh(Rectangle rectangle, WaveformMode mode);
    }
}