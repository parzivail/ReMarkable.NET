using ReMarkable.NET.Unix.Driver.Display.Framebuffer;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors;

namespace RmEmulator.Framebuffer
{
    public class EmulatedFramebuffer : IFramebuffer
    {
        private readonly EmulatorWindow _emulatorWindow;

        public int VirtualWidth => VisibleWidth;
        public int VirtualHeight => VisibleHeight;
        public int VisibleWidth { get; }
        public int VisibleHeight { get; }

        public static Image<Rgb24> BackBuffer;
        public static Image<Rgb24> FrontBuffer;

        public EmulatedFramebuffer(EmulatorWindow emulatorWindow, int visibleWidth, int visibleHeight)
        {
            _emulatorWindow = emulatorWindow;
            VisibleWidth = visibleWidth;
            VisibleHeight = visibleHeight;

            BackBuffer = new Image<Rgb24>(visibleWidth, visibleHeight);
            FrontBuffer = new Image<Rgb24>(visibleWidth, visibleHeight);
        }

        public Image<Rgb24> Read(Rectangle area)
        {
            ConstrainRectangle(ref area);
            return BackBuffer.Clone(i => i.Crop(area));
        }

        public void Write<TPixel>(Image<TPixel> image, Rectangle srcArea, Point destPoint) where TPixel : unmanaged, IPixel<TPixel>
        {
            image.Mutate(img => img.Grayscale(GrayscaleMode.Bt709, 1));

            var (x, y) = new Point(destPoint.X, destPoint.Y);
            ConstrainRectangle(ref srcArea, ref destPoint);
            srcArea.Location = new Point(srcArea.Location.X + destPoint.X - x, srcArea.Location.Y + destPoint.Y - y);

            BackBuffer.Mutate(backBuffer => backBuffer.DrawImage(image.Clone(srcImage => srcImage.Crop(srcArea)), destPoint, 1));
        }

        /// <inheritdoc />
        public void SetPixel(int x, int y, Color color)
        {
            BackBuffer[x, y] = color;
        }

        /// <inheritdoc />
        public void ConstrainRectangle(ref Rectangle srcArea, ref Point destPoint)
        {
            var tempRect = new Rectangle(destPoint, srcArea.Size);
            tempRect.Intersect(BackBuffer.Bounds());
            srcArea.Size = tempRect.Size;
            destPoint = tempRect.Location;
        }

        /// <inheritdoc />
        public void ConstrainRectangle(ref Rectangle area)
        {
            area.Intersect(BackBuffer.Bounds());
        }
    }
}