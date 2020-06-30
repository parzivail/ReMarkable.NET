using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using OpenToolkit.Graphics.OpenGL;
using ReMarkable.NET.Graphics;
using ReMarkable.NET.Unix.Driver.Display.Framebuffer;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace RmEmulator.Framebuffer
{
    public class EmulatedFramebuffer : IFramebuffer
    {
        public int VirtualWidth => VisibleWidth;
        public int VirtualHeight => VisibleHeight;
        public int VisibleWidth { get; }
        public int VisibleHeight { get; }

        public static Image<Rgb24> BackBuffer;

        public EmulatedFramebuffer(int visibleWidth, int visibleHeight)
        {
            VisibleWidth = visibleWidth;
            VisibleHeight = visibleHeight;

            BackBuffer = new Image<Rgb24>(visibleWidth, visibleHeight);
        }

        public Image<Rgb24> Read(Rectangle area)
        {
            return BackBuffer.Clone(i => i.Crop(area));
        }

        public void Write<TPixel>(Image<TPixel> image, Rectangle srcArea, Point destPoint) where TPixel : unmanaged, IPixel<TPixel>
        {
            BackBuffer.Mutate(backBuffer => backBuffer.DrawImage(image.Clone(srcImage => srcImage.Crop(srcArea)), destPoint, 1));

            EmulatorWindow.RefreshFlag = true;
        }
    }
}