using ReMarkable.NET.Unix.Driver.Display.EinkController;
using ReMarkable.NET.Unix.Driver.Display.Framebuffer;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ReMarkable.NET.Unix.Driver.Display
{
    /// <summary>
    ///     Provides an interface through which to access the device's display
    /// </summary>
    public interface IDisplayDriver
    {
        /// <summary>
        ///     The image data backing the display
        /// </summary>
        IFramebuffer Framebuffer { get; }

        /// <summary>
        ///     The vertical resolution of the backing framebuffer
        /// </summary>
        int VirtualHeight { get; }

        /// <summary>
        ///     The horizontal resolution of the backing framebuffer
        /// </summary>
        int VirtualWidth { get; }

        /// <summary>
        ///     The vertical resolution of the physically visible portion of the screen
        /// </summary>
        int VisibleHeight { get; }

        /// <summary>
        ///     The horizontal resolution of the physically visible portion of the screen
        /// </summary>
        int VisibleWidth { get; }

        /// <summary>
        ///     Draws image data to the screen and refreshes a portion of it
        /// </summary>
        /// <param name="image">The image to write to the screen</param>
        /// <param name="srcArea">The source area of the image to draw</param>
        /// <param name="destPoint">The point in the destination framebuffer where the source area will be drawn</param>
        /// <param name="refreshArea">The area of the screen to refresh. If omitted, will refresh the entire affected area.</param>
        /// <param name="waveformMode">
        ///     The waveform mode to use when refreshing the screen. If omitted, will use
        ///     <see cref="WaveformMode.Auto" />
        /// </param>
        /// <param name="displayTemp">The display temperature to use to refresh the region</param>
        /// <param name="updateMode">The update mode to use to refresh</param>
        void Draw(Image<Rgb24> image, Rectangle srcArea, Point destPoint, Rectangle refreshArea = default,
            WaveformMode waveformMode = WaveformMode.Auto, DisplayTemp displayTemp = DisplayTemp.Ambient, UpdateMode updateMode = UpdateMode.Partial);

        /// <summary>
        ///     Draws the contents of the internal buffer to the display device
        /// </summary>
        /// <param name="rectangle">The region of the buffer to be drawn to the device</param>
        /// <param name="mode">The update waveform used to refresh the region</param>
        /// <param name="displayTemp">The display temperature to use to refresh the region</param>
        /// <param name="updateMode">The update mode to use to refresh</param>
        void Refresh(Rectangle rectangle, WaveformMode mode, DisplayTemp displayTemp, UpdateMode updateMode);
    }
}