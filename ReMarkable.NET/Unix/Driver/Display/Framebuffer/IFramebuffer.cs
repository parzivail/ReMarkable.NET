using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ReMarkable.NET.Unix.Driver.Display.Framebuffer
{
    /// <summary>
    ///     Provides an interface through which to access the device's framebuffer
    /// </summary>
    public interface IFramebuffer
    {
        /// <summary>
        ///     The virtual height of the buffer
        /// </summary>
        int VirtualHeight { get; }

        /// <summary>
        ///     The virtual width of the buffer
        /// </summary>
        int VirtualWidth { get; }

        /// <summary>
        ///     The visible height of the buffer, starting from the top left
        /// </summary>
        int VisibleHeight { get; }

        /// <summary>
        ///     The visible width of the buffer, starting from the top left
        /// </summary>
        int VisibleWidth { get; }

        /// <summary>
        ///     Constrains a rectangle to not overlap the visible bounds of the display
        /// </summary>
        /// <param name="srcArea">The source area to constrain</param>
        /// <param name="destPoint">The point in the buffer where the source area should be placed</param>
        void ConstrainRectangle(ref Rectangle srcArea, ref Point destPoint);

        /// <summary>
        ///     Constrains a rectangle to not overlap the visible bounds of the display
        /// </summary>
        /// <param name="area">The source location and area to constrain</param>
        void ConstrainRectangle(ref Rectangle area);

        /// <summary>
        ///     Reads a rectangular region of pixels from the buffer
        /// </summary>
        /// <param name="area">The region of pixels to read</param>
        /// <returns>The pixels as an <see cref="Image{Rgb24}" /></returns>
        Image<Rgb24> Read(Rectangle area);

        /// <summary>
        ///     Writes a rectangular region of pixels to the buffer
        /// </summary>
        /// <typeparam name="TPixel">The pixel format</typeparam>
        /// <param name="image">The pixels to write to the buffer</param>
        /// <param name="srcArea">The source area of the image to draw</param>
        /// <param name="destPoint">The point in the buffer where the source area will be drawn</param>
        void Write<TPixel>(Image<TPixel> image, Rectangle srcArea, Point destPoint)
            where TPixel : unmanaged, IPixel<TPixel>;

        /// <summary>
        /// Sets exactly one pixel in the framebuffer to the specified color
        /// </summary>
        /// <param name="x">The X coordinate of the pixel</param>
        /// <param name="y">The Y coordinate of the pixel</param>
        /// <param name="color">The color to set the pixel to</param>
        void SetPixel(int x, int y, Color color);
    }
}