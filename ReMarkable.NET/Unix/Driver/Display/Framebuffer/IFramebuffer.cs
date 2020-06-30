using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ReMarkable.NET.Unix.Driver.Display.Framebuffer
{
    public interface IFramebuffer
    {
        int VirtualWidth { get; }
        int VirtualHeight { get; }
        int VisibleWidth { get; }
        int VisibleHeight { get; }

        Image<Rgb24> Read(Rectangle area);
        void Write<TPixel>(Image<TPixel> image, Rectangle srcArea, Point destPoint) where TPixel : unmanaged, IPixel<TPixel>;
    }
}